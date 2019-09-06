<template>
    <layout>

        <table class="table">
            <thead>
                <tr>
                    <th class="w-25">Name</th>
                    <th class="w-50">Description</th>
                    <th class="w-50">Shop Name</th>
                    <th class="w-25">Actions</th>
                </tr>
            </thead>
            <tbody v-if="!isLoading">
                <tr v-for="productCategory in productCategories" v-bind:key="productCategory.id">
                    <td>{{productCategory.name}}</td>
                    <td>{{productCategory.description}}</td>
                    <td>{{productCategory.shopName}}</td>
                    <td>
                        <button class="btn btn-danger" v-on:click="() => showDelete(productCategory)">Delete</button>
                    </td>
                </tr>
            </tbody>
        </table>

    </layout>
</template>
<script>
    import Api from "../../Api";
    import Layout from "../layout";

    export default {
        components: {
            Layout,
        },

        data() {
            return {
                isLoading: false,
                productCategories: []
            };
        },

        methods: {
            async loadAsync() {
                try {
                    this.isLoading = true;
                    let result = await Api.productCategories.getAllAsync();
                    this.productCategories = result.data;
                } finally {
                    this.isLoading = false;
                }
            }
        },

        mounted() {
            this.loadAsync();
        }

       
    };
</script>