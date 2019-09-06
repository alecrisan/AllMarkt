<template>
    <div v-if="on">
        <h1 style="text-transform:uppercase">{{shop.userDisplayName}}</h1>
        <div v-if="prodCategs.length !== 0">
            <div class="d-flex flex-wrap" id="productCategory">
                <ul class="nav nav-tabs nav-fill">
                    <li class="p-1">
                        <a class="nav-link disabled" style="text-emphasis-color:chartreuse">Product Category:</a>
                    </li>
                    <li class="nav-item p-1" title="All Categories" aria-expanded="true">
                        <a class="nav-link alert-primary" data-toggle="tab" v-on:click="loadProducts(0)">All Product Category</a>
                    </li>
                    <li class="nav-item p-1" v-for="prodCateg in prodCategs" v-bind:title="prodCateg.description" aria-expanded="true">
                        <a class="nav-link alert-primary" data-toggle="tab" v-on:click="loadProducts(prodCateg.id)">{{prodCateg.name}}</a>
                    </li>
                </ul>
            </div>
            <div id="products" v-if="products.length != 0 || firstDisplay">
                <table class="table table-hover">
                    <thead>
                        <tr>
                            <th>Picture</th>
                            <th>Name</th>
                            <th>Description</th>
                            <th>Price</th>
                            <th>State</th>
                            <th>Rating</th>
                            <th>Quantity</th>
                            <th class="d-flex justify-content-end">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="product in productsToDisplay" class="border border-dark" v-bind:key="product.id">
                            <td>poza</td>
                            <td>{{product.name}}</td>
                            <td>{{product.description.length<100 ? product.description : product.description.substr(0,97)+"..."}}</td>
                            <td>{{product.price}}</td>
                            <td>{{product.state?"Available" : "Out of Stock"}}</td>
                            <td>
                                <star-rating v-bind:increment="0.1"
                                             v-bind:star-size="20"
                                             :read-only="true"
                                             :show-rating="true"
                                             v-model="product.averageRating">
                                             
                                </star-rating>
                            </td>
                            <td>
                                <input :id="'ShopQuantity'+product.id" type="number" min="1" placeholder="1" style="width:50px"/>
                            </td>
                            <td class="d-flex justify-content-end">
                                <button class="btn btn-primary p-2 m-2" v-on:click="showProduct(product)">Details</button>
                                <button class="btn btn-primary p-2 m-2" v-on:click="addToCart(product)">Add to Cart</button>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div v-if="productsToDisplay.length !==0" class="d-flex flex-row justify-content-end" style="margin-right:20px">
                    <label>
                        <p>
                            Numar de produse pe Pagina: &nbsp&nbsp&nbsp
                        </p>
                    </label>
                    <div style="margin-right:20px">
                        <select class="form-control" id="exampleFormControlSelect1" v-model="nrItemsPerPage" v-on:change="getNumberOfPages();displayProducts(curentPage)">
                            <option>2</option>
                            <option>5</option>
                            <option>10</option>
                        </select>
                    </div>
                    <ul class="pagination ">
                        <li class="page-item" v-bind:class="isPrevDisabled()">
                            <a class="page-link alert-primary" v-on:click="getPrevProducts">Previous</a>
                        </li>
                        <li class="page-item" v-for="page in pageNr">
                            <a class="page-link" v-on:click="setPage(page);displayProducts(page)" v-bind:class="isCurrentPage(page)">{{page}}</a>
                        </li>
                        <li class="page-item" v-bind:class="isNextDisabled()">
                            <a class="page-link alert-primary" v-on:click="getNextProducts" v-bind:disabled="curentPage===pageNr">Next</a>
                        </li>
                    </ul>
                </div>
            </div>
            <div v-else>
                <p>
                    There are no products on this category.
                </p>
            </div>
        </div>
        <div v-else>
            <p>This shop is empty!</p>
        </div>

        <product-modal ref="productModal" />
    </div>
</template>

<script>
    import Api from "../../Api";
    import ProductModal from "../Customer/product-modal.vue";
    import StarRating from 'vue-star-rating'

    export default {
        components: {
            ProductModal,
            StarRating
        },

        data() {
            return {
                nrItemsPerPage: 2,
                curentPage: 1,
                firstDisplay: true,
                pageNr: 2,

                shop: {
                    id: 0,
                    userId: 0,
                    userDisplayName: "",
                    phoneNumber: "",
                    CUI: "",
                    IBAN: "",
                    address: "",
                    socialCapital: 0
                },
                prodCategs: [],
                products: [],
                productsToDisplay: [],

                on: false,
                id: 0,
                reviews: []
            };
        },
        methods: {
            turnOn() {
                this.on = true;
            },
            turnOff() {
                this.on = false;
            },
            async displayShop(shop) {
                this.firstDisplay = true;
                this.productsToDisplay = [];
                this.products = [];
                this.turnOn();
                this.shop = shop;
                await this.displayProductsCategory();
                await this.loadProducts(0);
            },

            async displayProductsCategory() {
                const result = await Api.productCategories.getAllBySelectedShopAsync(this.shop.id);
                this.prodCategs = result.data;
            },

            async loadProducts(id) {
                this.productsToDisplay = [];
                const result = (id ? await Api.products.getProductsWithRatingByCategoryIdAsync(id) : await Api.products.getAllBySelectedShopAsync(this.shop.id));
                this.products = result.data;
                this.firstDisplay = false;
                this.getNumberOfPages();
                this.displayProducts(this.curentPage);
            },

            displayProducts(pageNr) {
                this.productsToDisplay = [];
                const firstProductIndex = (pageNr - 1) * parseInt(this.nrItemsPerPage);
                var lastProductIndex = 0;
                if (firstProductIndex + parseInt(this.nrItemsPerPage) < this.products.length) {
                    lastProductIndex = firstProductIndex + parseInt(this.nrItemsPerPage) - 1;
                }
                else {
                    lastProductIndex = this.products.length - 1;
                }

                for (var i = firstProductIndex; i <= lastProductIndex; i++) {
                    this.productsToDisplay[i - firstProductIndex] = this.products[i];
                }
            },

            getNumberOfPages() {
                const result = Math.ceil(this.products.length / this.nrItemsPerPage);
                this.pageNr = result;
                this.curentPage = 1;
            },

            getPrevProducts() {
                if (this.curentPage > 1) {
                    this.curentPage--;
                }
                this.displayProducts(this.curentPage);
            },

            getNextProducts() {
                if (this.curentPage < this.pageNr) {
                    this.curentPage++;
                }
                this.displayProducts(this.curentPage);
            },

            setPage(page) {
                this.curentPage = page;
            },

            isPrevDisabled() {
                if (this.curentPage === 1)
                    return 'disabled';
                else return '';
            },
            isNextDisabled() {
                if (this.curentPage === this.pageNr)
                    return 'disabled';
                else return '';
            },

            isCurrentPage(page) {
                if (page === this.curentPage) {
                    return 'alert-secondary';
                }
            },

            showProduct(product) {
                this.$refs.productModal.show(product);
            },

            getStars(string){
                return parseFloat(string);
            },
            addToCart(product) {
                var quantity = (document.getElementById("ShopQuantity" + product.id).value ? document.getElementById("ShopQuantity" + product.id).value : 1);
                document.getElementById("ShopQuantity" + product.id).value = "";
                var item = { product: product, quantity: parseInt(quantity), shop: this.shop };
                this.$emit('addToCartShop', item);
            }
        }
    }
</script>