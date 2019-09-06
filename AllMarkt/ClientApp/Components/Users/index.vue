<template>
    <layout>
        <div class="d-flex align-items-center">
            <h2 class="flex-grow-1">Users</h2>
            <button class="btn btn-primary" v-on:click="showAdd">Add</button>
        </div>

        <table class="table">
            <thead>
                <tr>
                    <th class="w-25">Email</th>
                    <th class="w-50">Display Name</th>
                    <th class="w-25">User Role</th>
                </tr>
            </thead>
            <tbody v-if="!isLoading">
                <tr v-for="user in users" v-bind:key="user.id">
                    <td>{{user.email}}</td>
                    <td>{{user.displayName}}</td>
                    <td>{{user.userRole}}</td>
                    <td>
                        <button class="btn btn-primary" v-on:click="() => disableUser(user)">Disable</button>
                    </td>
                    <!--<td>
                        <button class="btn btn-danger" v-on:click="() => showDelete(user)">Delete</button>
                    </td>-->
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
                users: []
            };
        },

        methods: {
            showAdd() {
                this.$refs.formModal.showAdd();
            },

            showEdit(user) {
                this.$refs.formModal.showEdit(user);
            },

            disableUser(user) {
                this.$refs.deleteModal.show(user);
            },

            async loadAsync() {
                try {
                    this.isLoading = true;
                    let result = await Api.users.getAllAsync();
                    this.users = result.data;
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