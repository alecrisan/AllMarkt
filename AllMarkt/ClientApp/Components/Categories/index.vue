<template>
    <layout>
        <div class="d-flex align-items-center">
            <h2 class="flex-grow-1">Categories</h2>
            <button v-if="userRole === 'Admin'" class="btn btn-primary" v-on:click="showAdd">Add</button>
        </div>

        <table class="table">
            <thead>
                <tr>
                    <th class="w-25">Name</th>
                    <th class="w-50">Description</th>
                    <th class="w-25">Actions</th>
                </tr>
            </thead>
            <tbody v-if="!isLoading">
                <tr v-for="category in categories" v-bind:key="category.id">
                    <td>{{category.name}}</td>
                    <td>{{category.description}}</td>
                    <td>
                        <button v-if="userRole === 'Admin'" class="btn btn-danger" v-on:click="() => showDelete(category)">Delete</button>
                        <button v-if="userRole ==='Shop'" class="btn btn-primary" v-on:click="()=>addShopLink(category.id)">Add Shop to this Category</button>
                        <button v-if="userRole ==='Shop'" class="btn btn-danger" v-on:click="()=>deleteShopLink(category.id)"> Delete Shop from this Category</button>
                    </td>
                </tr>
            </tbody>
        </table>

        <div v-if="isLoading" class="d-flex justify-content-center">
            <div class="spinner-border text-info">
                <span class="sr-only">Loading...</span>
            </div>
        </div>

        <form-modal ref="formModal" v-on:submitted="loadAsync" />
        <delete-modal ref="deleteModal" v-on:submitted="loadAsync" />
    </layout>
</template>
<script>
    import Api from "../../Api";
    import Layout from "../Layout";
    import FormModal from "./form-modal";
    import DeleteModal from "./delete-modal";
    import UserStore from "../../UserStore";
    export default {
        components: {
            Layout,
            FormModal,
            DeleteModal
        },

        data() {
            return {
                isLoading: false,
                categories: [],
                userRole: ""
            };
        },

        methods: {
            async addShopLink(categoryId) {
                try {
                    await Api.shopCategory.addShopCategoryLink(categoryId);
                    alert("Your Shop has been added to this Category!");
                } catch (exception) {
                    alert("Your shop is already in this Category!");
                }

            },
            async  deleteShopLink(categoryId) {
                try {
                    await Api.shopCategory.deleteShopCategoryLink(categoryId);
                    alert("Your Shop has been deleted from this Category!");
                } catch (exception) {
                    alert("Your Shop has already been deleted from this Category!");
                }

            },

            showAdd() {
                this.$refs.formModal.show();
            },

            showDelete(category) {
                this.$refs.deleteModal.show(category);
            },

            async loadAsync() {
                try {
                    this.isLoading = true;
                    let result = await Api.categories.getAllAsync();
                    this.categories = result.data;
                } finally {
                    this.isLoading = false;
                }
            }
        },

        mounted() {
            this.loadAsync();
            var currentUser = JSON.parse(localStorage.getItem("currentUser"));
            this.userRole = currentUser.userRole;
            UserStore.userRole = this.userRole;
        }
    };
</script>
