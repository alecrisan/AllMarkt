<template>
    <div>
        <ShopPage ref="shopPage" v-on:addToCartShop="addToCartMain" />
        <div v-if="!browsingShop">
            <div v-if="isLoading" class="d-flex justify-content-center">
                <div class="spinner-border text-info">
                    <span class="sr-only">Loading...</span>
                </div>
            </div>
            <div class="row">
                <div v-if="!isLoading" class="col-md-3 mb-3 d-flex align-items-stretch" v-for="shop in shops" v-bind:key="shop.id">
                    <div class="card" style="width:18rem" v-on:click="()=>showShopPage(shop)">
                        <div class="card-body">
                            <h3 class="card-title card-header" style="text-align:center;">{{shop.userDisplayName}}</h3>
                            <p class="card-text">Address: {{shop.address}}</p>
                            <p class="card-text">Phone Number: {{shop.phoneNumber}}</p>
                        </div>
                    </div>

                </div>
                <div v-if="shops.length===0 && !isLoading">
                    <h4 style="margin-left:50px">There are no shops in this category!</h4>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import Api from "../../Api";
    import ShopPage from "../Customer/shopPage.vue";
    export default {
        components: {
            ShopPage
        },

        data() {
            return {
                shopId: 0,
                shops: [],
                isLoading: false,
                browsingShop: false
            };
        },

        methods: {
            async loadShopsAsync(categoryid) {
                this.browsingShop = false;
                this.$refs.shopPage.turnOff();
                try {
                    this.isLoading = true;
                    const result = await (categoryid ? Api.shops.getShopsByCategoryIdAsync(categoryid) : Api.shops.getAllShopsAsync());

                    this.shops = result.data;
                } finally {
                    this.isLoading = false;
                }
            },
            showShopPage(shop) {
                this.browsingShop = true;
                this.$refs.shopPage.displayShop(shop);
            },
            addToCartMain(item) {
                this.$emit('addToCartIndex', item);
            }
        },

        mounted() {
            this.loadShopsAsync();
        }
    }
</script>
