<template>
    <layout>
        <div class="d-flex align-items-center">
            <h2 class="flex-grow-1">Product Comments</h2>
            <!--<button class="btn btn-primary" v-on:click="showAdd">Add</button>-->
        </div>
        <table class="table">
            <thead>
                <tr>
                    <th> Product Name</th>
                    <th> Rating</th>
                    <th> Text</th>
                    <th> Added by</th>
                    <th> Actions </th>
                </tr>
            </thead>
            <tbody v-if="!isLoading">
                <tr v-for="productComment in productComments" v-bind:key="productComment.id">
                    <td>{{productComment.productName}}</td>
                    <td><star-rating v-bind:show-rating="false" v-bind:rating="productComment.rating" v-bind:star-size="25" v-bind:read-only="true"></star-rating></td>
                    <td>{{productComment.text}}</td>
                    <td>{{productComment.addedByName}}</td>
                    <td>
                        <button type="button" class="btn btn-primary" v-on:click="() => showEdit(productComment)">Edit</button>
                        <button class="btn btn-danger" v-on:click="() => showDelete(productComment)">Delete</button>
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
                productComments: []
            };
        },
        methods: {
            showAdd() {
                this.$refs.formModal.show();
            },

            showDelete(productComment) {
                this.$refs.deleteModal.show(productComment);
            },

            showEdit(productComment) {
                this.$refs.formModal.showEdit(productComment);
            },

            async loadAsync() {
                try {
                    this.isLoading = true;
                    let result = await Api.productComments.getAllAsync();
                    this.productComments = result.data;
                }
                finally {
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