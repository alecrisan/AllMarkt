<template>
    <layout>
        <div class="d-flex align-items-center">
            <h2 class="flex-grow-1">Shop Comments</h2>
            <button class="btn btn-primary" v-on:click="showAdd">Add</button>
        </div>
        <table class="table">
            <thead>
                <tr>
                    <th> Shop Name</th>
                    <th> Rating</th>
                    <th> Text</th>
                    <th> Added by</th>
                    <th> Actions</th>
                </tr>
            </thead>
            <tbody v-if="!isLoading">
                <tr v-for="shopComment in shopComments" v-bind:key="shopComment.id">
                    <td>{{shopComment.shopName}}</td>
                    <td><star-rating v-bind:show-rating="false" v-bind:rating="shopComment.rating" v-bind:star-size="25" v-bind:read-only="true"></star-rating></td>
                    <td>{{shopComment.text}}</td>
                    <td>{{shopComment.addedByName}}</td>
                    <td>
                        <button class="btn btn-primary" v-on:click="() => showEdit(shopComment)">Edit</button>
                        <button class="btn btn-danger" v-on:click="() => showDelete(shopComment)">Delete</button>
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
                shopComments: []
            };
        },
        methods: {
            showAdd() {
                this.$refs.formModal.show();
            },
            showDelete(shopComment) {
                this.$refs.deleteModal.show(shopComment);
            },
            showEdit(shopComment) {
                this.$refs.formModal.showEdit(shopComment);
            },

            async loadAsync() {
                try {
                    this.isLoading = true;
                    let result = await Api.shopComments.getAllAsync();
                    this.shopComments = result.data;
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