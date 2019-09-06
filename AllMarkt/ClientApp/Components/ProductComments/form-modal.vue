<template>
    <div class="modal fade">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 v-if="isEdit===true" class="modal-title">Edit Comment</h5>
                    <h5 v-else class="modal-tile"> Add Comment</h5>
                    <button type="button" class="close" v-on:click="hide">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div v-if="isLoading" class="d-flex justify-content-center">
                        <div class="spinner-border text-info">
                            <span class="sr-only">Loading...</span>
                        </div>
                    </div>

                    <form v-else>
                        <div class="form-group">
                            <div>
                                <star-rating v-model="productComment.rating"
                                             v-on:input="validateRating"
                                             v-on:blur="validateRating"
                                             v-bind:class="ratingValidClass"
                                             v-bind:read-only=isEdit
                                             v-bind:show-rating="false"
                                             class="form-control"
                                             id="rating"
                                             v-bind:star-size="35"
                                             @rating-selected="setRating">
                                </star-rating>
                                <div class="invalid-feedback">{{this.productComment.ratingError}}</div>
                            </div>

                        </div>

                        <div class="form-group">
                            <label for="text">Comment</label>
                            <textarea v-model="productComment.text"
                                      v-on:input="validateText"
                                      v-on:blur="validateText"
                                      v-bind:class="textValidClass"
                                      class="form-control"
                                      id="text"
                                      rows="3" />
                            <div class="invalid-feedback">{{this.productComment.textError}}</div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button"
                            class="btn btn-primary"
                            v-on:click="saveAsync"
                            v-bind:disabled="isLoading || !productComment.isValid">
                        Save
                    </button>
                    <button type="button"
                            class="btn btn-secondary"
                            v-on:click="hide"
                            v-bind:disabled="isLoading">
                        Cancel
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import $ from "jquery";
    import Api from "../../Api";

    export default {
        computed: {
            ratingValidClass() {
                if (this.productComment.ratingError === null) return "";
                else if (this.productComment.ratingError === "") return "is-valid";
                else return "is-invalid";
            },
            textValidClass() {
                if (this.productComment.textError === null) return "";
                else if (this.productComment.textError === "") return "is-valid";
                else return "is-invalid";
            }
        },

        data() {
            return {
                isEdit: false,
                isLoading: false,
                productComment: {
                    rating: 0,
                    text: "",
                    userid: 1,
                    productid: 1,
                    ratingError: null,
                    isValid: null,
                    textError: null
                }
            };
        },

        methods: {
            show() {
                this.isEdit = false;
                this.productComment = {
                    rating: 0,
                    text: "",
                    userid: 1,
                    productid: 1,
                    ratingError: null,
                    isValid: null,
                    textError: null
                };
                $(this.$el).modal("show");
            },

            showEdit(productComment) {
                this.isEdit = true;
                this.productComment.isValid = true;
                for (var key in productComment) {
                    this.productComment[key] = productComment[key];
                }
                $(this.$el).modal("show");
            },

            hide() {
                $(this.$el).modal("hide");
            },

            setRating: function (rating) {
                this.productComment.rating = rating;
                this.validateRating();
            },

            validateRating() {
                this.productComment.ratingError = "";
                if (this.productComment.rating === 0)
                    this.productComment.ratingError = "Please provide a rating";
                this.productComment.isValid = (this.productComment.textError === "" || this.productComment.textError === null) && this.productComment.ratingError === "";
            },
            validateText() {
                this.validateRating();
                this.productComment.textError = "";
                if (this.productComment.text.length > 1023)
                    this.productComment.textError = "Comment too long (max 1023 characters)";
                if (this.isEdit === true) this.productComment.isValid = this.productComment.textError === "";
                else
                    this.productComment.isValid = (this.productComment.textError === "" || this.productComment.textError === null) && this.productComment.ratingError === "";
            },

            async saveAsync() {
                try {
                    this.isLoading = true;
                    if (this.isEdit === false) {
                        await Api.productComments.addAsync({
                            rating: this.productComment.rating,
                            text: this.productComment.text,
                            addedById: this.productComment.userid,
                            productId: this.productComment.productid,
                        });
                    }
                    else {
                        await Api.productComments.editAsync({
                            id: this.productComment.id,
                            rating: this.productComment.rating,
                            text: this.productComment.text,
                            addedById: this.productComment.addedById,
                            productId: this.productComment.productId,
                        });
                    }
                    this.hide();
                    this.$emit("submitted");
                } finally {
                    this.isLoading = false;
                }
            }
        }
    };

    $(document).ready(function () {
        $('[data-toggle="popover"]').popover();
    });
</script>
