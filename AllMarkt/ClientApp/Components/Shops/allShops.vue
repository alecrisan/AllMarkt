<template>
    <layout>
        <table class="table">
            <thead>
                <tr>
                    <th class="w-10">Display Name</th>
                    <th class="w-10">Actions</th>
                </tr>
            </thead>
            <tbody v-if="!isLoading">
                <tr v-for="shop in shops" v-bind:key="shop.id">
                    <td>{{shop.userDisplayName}}</td>
                    <td>
                        <router-link :to="'/productCategories/shop/' + shop.id" class="nav-link" active-class="active">Product Categories</router-link>
                    </td>
                </tr>
            </tbody>
        </table>
        <div v-if="isLoading" class="d-flex justify-content-center">
            <div class="spinner-border text-info">
                <span class="sr-only">Loading...</span>
            </div>
        </div>

    </layout>
</template>
<script>
    import Api from "../../Api";
    import Layout from "../Layout";
    import ByShop from "../ProductCategories/byShop";
    import UserStore from "../../UserStore";

    export default {
        components: {
            Layout,
            ByShop
        },

        data() {
            return {
                isLoading: false,
                shops: []
            }
        },
      

        methods: {
            async loadAsync() {
                try {
                    this.isLoading = true;
                    let result = await Api.shops.getAllShopsAsync();
                    this.shops = result.data;
                } finally {
                    this.isLoading = false;
                }
            }
        },
        mounted() {
            this.loadAsync();
              var currentUser = JSON.parse(localStorage.getItem("currentUser"));
            UserStore.userRole = currentUser.userRole;
        }
    };
</script>