<template>
    <layout>
        <div class="d-flex align-items-center">
            <h2 v-if="pageType === 'sent'" class="flex-grow-1">Sent Private Messages</h2>
            <h2 v-if="pageType === 'received'" class="flex-grow-1">Received Private Messages</h2>
            <h2 v-if="pageType === 'admin'" class="flex-grow-1">Admin Control Page For All Private Messages</h2>
            <button v-if="pageType === 'sent'" class="btn btn-primary" v-on:click="() => showAdd()">Add</button>
        </div>
        <table class="table">
            <thead>
                <tr>
                    <th style="width:8%">Sender</th>
                    <th style="width:8%">Receiver</th>
                    <th style="width:15%">Title</th>
                    <th style="width:45%">Text</th>
                    <th style="width:9%">Date Sent</th>
                    <th style="width:5%">Status</th>
                    <th style="width:10%">Action</th>
                </tr>
            </thead>
            <tbody v-if="!isLoading">
                <tr v-for="privateMessage in privateMessages" v-bind:key="privateMessage.id">
                    <td>{{privateMessage.sender.displayName}}</td>
                    <td>{{privateMessage.receiver.displayName}}</td>
                    <td>{{privateMessage.title}}</td>
                    <td v-html="formatText(privateMessage.text)"></td>
                    <td>{{privateMessage.dateSent.toString().slice(0, 16).replace(/-/g, "/").replace("T", " ")}}</td>
                    <td>{{privateMessage.dateRead === null ? "Unread" : "Read"}}</td>
                    <td>
                        <button v-if="pageType === 'admin' || pageType === 'sent'" class="btn btn-primary" v-on:click="() => showEdit(privateMessage)">Edit</button>
                        <button class="btn btn-danger" v-on:click="() => showDelete(privateMessage)">Delete</button>
                    </td>
                </tr>
            </tbody>
        </table>

        <div v-if="isLoading" class="d-flex justify-content-center">
            <div class="spinner-border text-info">
                <span class="sr-only">Loading...</span>
            </div>
        </div>
        <form-modal :type=formType ref="formModal" v-on:submitted="loadAsync" />
        <delete-modal ref="deleteModal" v-on:submitted="loadAsync" />
    </layout>
</template>
<script>
    import Api from "../../Api";
    import Layout from "../Layout";
    import FormModal from "./form-modal.vue";
    import DeleteModal from "./delete-modal.vue";
import UserStore from "../../UserStore";

    export default {
        props: {
            "pageType": {
                default: false
            }
        },

        components: {
            Layout,
            FormModal,
            DeleteModal
        },

        data() {
            return {
                formType: null,
                isLoading: false,
                privateMessages: []
            };
        },

        methods:
        {
            async loadAsync() {                
                try {
                    this.isLoading = true;
                    var result = null;
                    if (this.pageType === "sent") {
                        result = await Api.privateMessages.getAllSentPrivateMessagesByUserAsync();
                    }
                    else if (this.pageType === "received") {
                        result = await Api.privateMessages.getAllReceivedPrivateMessagesByUserAsync();
                    }
                    else if (this.pageType === "admin") {
                        result = await Api.privateMessages.getAllPrivateMessagesAsync();
                    }
                    this.privateMessages = result.data;
                }
                finally {
                    this.isLoading = false;
                }
            },

            showAdd() {
                this.formType = "add";
                this.$refs.formModal.showAdd();
            },

            showEdit(privateMessage) {
                this.formType = "edit";
                this.$refs.formModal.showEdit(Object.assign({}, privateMessage));
            },

            showDelete(privateMessage) {
                this.$refs.deleteModal.show(privateMessage, "delete");
            },

            formatText(text) {
                return text.replace(/\n/g, '<br>');
            }
        },

        mounted() {
            this.loadAsync();
            var currentUser = JSON.parse(localStorage.getItem("currentUser"));
            UserStore.userRole = currentUser.userRole;
        }
    };
</script>