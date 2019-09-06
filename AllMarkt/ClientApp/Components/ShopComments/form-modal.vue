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
                            <label for="rating">Rating</label>
                            <div>
                                <star-rating v-model="shopComment.rating"
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
                            </div>
                            <div class="invalid-feedback">{{this.shopComment.ratingError}}</div>
                        </div>
                        <div class="form-group">
                            <label for="text">Comment</label>
                            <textarea v-model="shopComment.text"
                                      v-on:input="validateText"
                                      v-on:blur="validateText"
                                      v-bind:class="textValidClass"
                                      class="form-control"
                                      id="text"
                                      rows="3" />
                            <div class="invalid-feedback">{{this.shopComment.textError}}</div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button"
                            class="btn btn-primary"
                            v-on:click="saveAsync"
                            v-bind:disabled="isLoading || !shopComment.isValid"
                            v-bind:enabled="shopComment.length !== 0">
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
                if (this.shopComment.ratingError === null) return "";
                else if (this.shopComment.ratingError === "") return "is-valid";
                else return "is-invalid";
            },
            textValidClass() {
                if (this.shopComment.textError === null) return "";
                else if (this.shopComment.textError === "") return "is-valid";
                else return "is-invalid";
            }
        },

        data() {
            return {
                isEdit: false,
                isLoading: false,
                shopComment: {
                    rating: 0,
                    text: "",
                    userid: 1,
                    shopid: 1,
                    ratingError: null,
                    isValid: null,
                    textError: null
                }
            };
        },

        methods: {
            show() {
                this.isEdit = false;
                this.shopComment = {
                    rating: 0,
                    text: "",
                    userid: 1,
                    shopid: 1,
                    ratingError: null,
                    isValid: null,
                    textError: null
                };
                $(this.$el).modal("show");
            },

            showEdit(shopComment) {
                this.isEdit = true;
                this.shopComment.isValid = true;
                for (var key in shopComment) {
                    this.shopComment[key] = shopComment[key];
                }
                $(this.$el).modal("show");
            },

            hide() {
                $(this.$el).modal("hide");
            },

            validateRating() {
                this.shopComment.ratingError = "";
                if (this.shopComment.rating === 0)
                    this.shopComment.ratingError = "Please provide a rating";

                this.shopComment.isValid = (this.shopComment.textError === "" || this.shopComment.textError === null) && this.shopComment.ratingError === "";
            },
            validateText() {
                this.validateRating();
                this.shopComment.textError = "";
                if (this.shopComment.text.length > 1023)
                    this.shopComment.textError = "Comment too long (max 1023 characters)";

                if (this.isEdit === true) this.shopComment.isValid = this.shopComment.textError === "";
                else
                    this.shopComment.isValid = (this.shopComment.textError === "" || this.shopComment.textError === null) && this.shopComment.ratingError === "";


            },

            setRating: function (rating) {
                this.shopComment.rating = rating;
                this.validateRating();
            },

            async saveAsync() {
                try {
                    this.isLoading = true;
                    if (this.isEdit === false) {
                        await Api.shopComments.addAsync({
                            rating: this.shopComment.rating,
                            text: this.shopComment.text,
                            addedById: this.shopComment.userid,
                            shopId: this.shopComment.shopid,
                        });
                    }
                    else {
                        await Api.shopComments.editAsync({
                            id: this.shopComment.id,
                            rating: this.shopComment.rating,
                            text: this.shopComment.text,
                            addedById: this.shopComment.addedById,
                            shopId: this.shopComment.shopid,
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
</script>
