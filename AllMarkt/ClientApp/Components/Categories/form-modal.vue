<template>
    <div class="modal fade">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Add Category</h5>
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
                            <label for="name">Name</label>
                            <input v-model="category.name"
                                   v-on:input="validateName"
                                   v-on:blur="validateName"
                                   type="text"
                                   class="form-control"
                                   v-bind:class="nameValidClass"
                                   id="name"
                                   placeholder="E.g.: cars, books, food etc." />
                            <div class="invalid-feedback">{{this.category.nameError}}</div>
                        </div>

                        <div class="form-group">
                            <label for="description">Description</label>
                            <textarea v-model="category.description"
                                      class="form-control"
                                      id="description"
                                      rows="3" />
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button"
                            class="btn btn-primary"
                            v-on:click="saveAsync"
                            v-bind:disabled="isLoading || !category.isValid">
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
            nameValidClass() {
                if (this.category.isValid === null) return "";
                else if (this.category.isValid) return "is-valid";
                else return "is-invalid";
            }
        },

        data() {
            return {
                isLoading: false,
                category: {
                    name: "",
                    description: "",
                    nameError: null,
                    isValid: null
                }
            };
        },

        methods: {
            show() {
                this.category = {
                    name: "",
                    description: "",
                    nameError: null,
                    isValid: null
                };
                $(this.$el).modal("show");
            },

            hide() {
                $(this.$el).modal("hide");
            },

            validateName() {
                this.category.nameError = null;
                if (this.category.name.trim().length === 0)
                    this.category.nameError = "Please provide a name";
                else if (this.category.name.length > 80)
                    this.category.nameError = "Name too long (max 80 characters)";

                this.category.isValid = this.category.nameError === null;
            },

            async saveAsync() {
                try {
                    this.isLoading = true;
                    await Api.categories.addAsync({
                        name: this.category.name,
                        description: this.category.description
                    });
                    this.hide();
                    this.$emit("submitted");
                } finally {
                    this.isLoading = false;
                }
            }
        }
    };
</script>
