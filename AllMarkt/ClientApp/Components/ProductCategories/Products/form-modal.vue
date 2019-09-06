<template>
    <div class="modal fade">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 v-if="onAdd" class="modal-title">Add Product</h5>
                    <h5 v-else class="modal-title">Edit Product</h5>
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
                            <input v-model="product.name"
                                   v-on:input="validateName"
                                   v-on:blur="validateName"
                                   type="text"
                                   class="form-control"
                                   v-bind:class="nameValidClass"
                                   id="name" />
                            <div class="invalid-feedback">{{this.product.nameError}}</div>
                        </div>

                        <div class="form-group">
                            <label for="description">Description</label>
                            <textarea v-model="product.description"
                                      v-on:input="validateDescription"
                                      v-on:blur="validateDescription"
                                      class="form-control"
                                      v-bind:class="descriptionValidClass"
                                      id="description"
                                      rows="3" />
                            <div class="invalid-feedback">{{this.product.descriptionError}}</div>
                        </div>

                        <div class="form-group">
                            <label for="price">Price</label>
                            <input v-model="product.price"
                                   v-on:input="validatePrice"
                                   v-on:blur="validatePrice"
                                   type="text"
                                   class="form-control"
                                   v-bind:class="priceValidClass"
                                   id="price" />
                            <div class="invalid-feedback">{{this.product.priceError}}</div>
                        </div>

                        <div v-if="!onAdd" class="form-group">
                            <label for="state">State</label>
                            <br />
                            <select class="form-control" v-model="selectedState" id="state">
                                <option value="Available">Available</option>
                                <option value="Unavailable">Unavailable</option>
                            </select>
                        </div>

                        <div v-if="onAdd" class="form-group">
                            <label for="productCategory">Product Category</label>
                            <br />
                            <select class="form-control" id="productCategoryId" v-model="product.productCategoryId">
                                <option value="" disabled selected>Please select a category</option>
                                <option v-for="productCategory in productCategories" v-bind:value="productCategory.id" v-on:click="selectCategory(productCategory)">{{productCategory.name}}</option>
                            </select>
                        </div>

                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text" id="inputGroupFileAddon01">Upload</span>
                            </div>
                            <div class="custom-file">
                                <input type="file" class="custom-file-input" id="imageURI"
                                       aria-describedby="inputGroupFileAddon01">
                                <label class="custom-file-label" for="inputGroupFile01">Choose file</label>
                            </div>
                        </div>

                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button"
                            class="btn btn-primary"
                            v-on:click="saveAsync"
                            v-bind:disabled="isLoading || !product.isValid">
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
                if (this.product.nameError === null) return "";
                else if (this.product.nameError === "") return "is-valid";
                else return "is-invalid";
            },
            descriptionValidClass() {
                if (this.product.descriptionError === null)
                    return "";
                else if (this.product.descriptionError === "")
                    return "is-valid";
                else
                    return "is-invalid";
            },
            priceValidClass() {
                if (this.product.priceError === null)
                    return "";
                else if (this.product.priceError === "")
                    return "is-valid";
                else
                    return "is-invalid";
            }
        },

        data() {
            return {
                onAdd: true,
                isLoading: false,
                product: {
                    name: "",
                    description: "",
                    price: "",
                    imageURI: "",
                    state: "",
                    productCategoryId: "",
                    productCategoryName: "",
                    nameError: null,
                    descriptionError: null,
                    priceError: null,
                    isValid: null
                },
                productCategories: [],
                selectedCategory: 0,
                selectedState: "Available"
            };
        },

        methods: {
            async showAdd() {
                this.onAdd = true;
                this.product = {
                    name: "",
                    description: "",
                    price: "",
                    imageURI: "",
                    state: "",
                    productCategoryName: "",
                    nameError: null,
                    descriptionError: null,
                    priceError: null,
                    isValid: null
                };

                await this.loadCategoriesAsync();

                $(this.$el).modal("show");
            },

            async showEdit(product) {
                this.onAdd = false;
                this.product.isValid = true;
                this.product.id = product.id;
                this.product.name = product.name;
                this.product.description = product.description;
                this.product.price = product.price;
                this.product.productCategoryId = product.productCategoryId;

                if (product.state === false)
                    this.selectedState = "Unavailable";

                this.product.state = this.selectedState;

                this.product.nameError = "";
                this.product.descriptionError = "";
                this.product.priceError = "";

                $(this.$el).modal("show");
            },

            async loadCategoriesAsync() {
                let result = await Api.productCategories.getAllAsync();
                this.productCategories = result.data;
            },

            hide() {
                console.log(this.isValid);
                this.selectedState = "Available";
                $(this.$el).modal("hide");
            },

            selectCategory(productCategory) {
                this.product.productCategoryId = productCategory.id;
                this.product.productCategoryName = productCategory.name;
                console.log(this.product.productCategoryName);
            },

            validateName() {
                this.product.nameError = "";

                if (this.product.name.trim().length === 0)
                    this.product.nameError = "Please provide a name";
                else if (this.product.name.length > 50)
                    this.product.nameError = "Name too long (max 50 characters)";

                this.product.isValid = this.product.nameError === "";
            },

            validateDescription() {
                this.product.descriptionError = "";

                if (this.product.description.trim().length === 0)
                    this.product.descriptionError = "Please provide a description";

                else if (this.product.description.length > 255)
                    this.product.descriptionError = "Description too long (max 255 characters)";

                this.product.isValid = this.product.descriptionError === "";
            },

            validatePrice() {
                this.product.priceError = "";

                if (this.product.price.trim().length === 0)
                    this.product.priceError = "Please provide a price";

                this.product.isValid = this.product.priceError === "";
            },

            async saveAsync() {
                try {
                    this.isLoading = true;

                    if (this.onAdd === true)
                        await this.addAsync();
                    else
                        await this.editAsync();

                    this.hide();
                    this.$emit("submitted");
                } finally {
                    this.isLoading = false;
                }
            },

            async addAsync() {
                await Api.products.addAsync({
                        name: this.product.name,
                        description: this.product.description,
                        price: this.product.price,
                        imageURI: this.product.imageURI,
                        state: true,
                        productCategoryId: this.product.productCategoryId
                    });
            },

             async editAsync() {
                    if (this.selectedState === "Unavailable")
                        this.product.state = false;
                    else
                        this.product.state = true;

                    await Api.products.editAsync({
                        id: this.product.id,
                        name: this.product.name,
                        description: this.product.description,
                        price: this.product.price,
                        imageURI: this.product.imageURI,
                        state: this.product.state,
                        productCategoryId: this.product.productCategoryId
                    });
            }
        }
    };
</script>
