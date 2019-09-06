
<template>
    <layout>


        <div class="mx-5 my-3">
            <h1>Private Messages</h1>

            <div class="container">
                <div class="row">
                    <div class="col-1">
                        <div class="btn-group-vertical">
                            <button type="button" class="btn btn-primary" v-on:click="() => showInbox()">Inbox</button>
                            <button type="button" class="btn btn-primary" v-on:click="() => showSent()">Sent</button>
                            <button type="button" class="btn btn-primary" v-on:click="() => createMessage()">Create</button>
                        </div>
                    </div>

                    <div class="col-10">
                        <table class="table">
                            <thead>
                                <tr>
                                    <th style="width:15%" v-if="isInbox">From</th>
                                    <th style="width:15%" v-else>To</th>
                                    <th style="width:20%">Subject</th>
                                    <th style="width:10%">Date Sent</th>
                                    <th style="width:10%">Status</th>
                                    <th style="width:10%">Action</th>
                                </tr>
                            </thead>

                            <tbody v-if="!isLoading">
                                <tr v-for="privateMessage in privateMessages" v-bind:key="privateMessage.id">
                                    <td v-if="isInbox">{{privateMessage.sender.displayName}}</td>
                                    <td v-else>{{privateMessage.receiver.displayName}}</td>
                                    <td v-on:click="() => showMessage(privateMessage)"><a href="#">{{privateMessage.title}}</a></td>
                                    <td>{{privateMessage.dateSent.toString().slice(0, 16).replace(/-/g, "/").replace("T", " ")}}</td>
                                    <td>{{privateMessage.dateRead === null ? "Unread" : "Read"}}</td>
                                    <td>
                                        <button class="btn btn-danger" v-on:click="() => showDelete(privateMessage)">Delete</button>
                                        <button v-if="isInbox" type="button" class="btn btn-primary" v-on:click="() => reply(privateMessage)">Reply</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>

                        <div v-if="isInboxEmpty && isInbox" class="d-flex justify-content-center">You have no received messages</div>
                        <div v-if="isSentEmpty && isSent" class="d-flex justify-content-center">You have not sent any messages</div>

                        <div v-if="isLoading" class="d-flex justify-content-center">
                            <div class="spinner-border text-info">
                                <span class="sr-only">Loading...</span>
                            </div>
                        </div>
                    </div>
                </div>


            </div>
            <form-modal :type=formType ref="formModal" v-on:submitted="showInbox" />
            <show-modal :type=formType ref="showModal" v-on:submitted="showInbox" />
            <delete-modal ref="deleteModal" v-on:submitted="showInbox" />
        </div>
    </layout>
</template>
<script>
    import Api from "../../Api";
    import FormModal from "./form-modal.vue";
    import ShowModal from "./show-modal.vue";
    import DeleteModal from "./delete-modal.vue";
    import Layout from "../Layout";
    import UserStore from "../../UserStore";

    export default {
        components: {
            FormModal,
            ShowModal,
            DeleteModal,
            Layout
        },

        data() {
            return {
                formType: "add",
                isLoading: false,
                isInbox: true,
                isSent: false,
                isInboxEmpty: false,
                isSentEmpty: false,
                privateMessages: []
            };
        },

        methods:
        {
            async showInbox() {
                this.isLoading = true;
                this.isInbox = true;
                this.isSent = false;
                this.isInboxEmpty = false;
                var result = await Api.privateMessages.getAllReceivedPrivateMessagesByUserAsync();
                this.privateMessages = result.data;
                if (this.privateMessages.length === 0)
                    this.isInboxEmpty = true;
                this.isLoading = false;
            },

            async showSent() {
                this.isLoading = true;
                this.isSent = true;
                this.isInbox = false;
                this.isSentEmpty = false;
                var result = await Api.privateMessages.getAllSentPrivateMessagesByUserAsync();
                this.privateMessages = result.data;
                if (this.privateMessages.length === 0)
                    this.isSentEmpty = true;
                this.isLoading = false;
            },

            async createMessage() {
                await this.$refs.formModal.showAdd(null);
            },

            async showMessage(privateMessage) {
                await this.$refs.showModal.show(privateMessage, this.isInbox);
            },

            async showDelete(privateMessage) {
                if (this.isInbox) {
                    await this.$refs.deleteModal.show(privateMessage, 1);
                } else {
                    await this.$refs.deleteModal.show(privateMessage, 0);
                }
            },

            dateFormatter(dateTime) {
                var date = dateTime.getFullYear() + '-' +
                    ((dateTime.getMonth() + 1) < 10 ? '0' + (dateTime.getMonth() + 1) : (dateTime.getMonth() + 1)) + '-' +
                    (dateTime.getDate() < 10 ? '0' + dateTime.getDate() : dateTime.getDate());

                var time = (dateTime.getHours() < 10 ? '0' + dateTime.getHours() : dateTime.getHours()) + ":" +
                    (dateTime.getMinutes() < 10 ? '0' + dateTime.getMinutes() : dateTime.getMinutes()) + ":" +
                    (dateTime.getSeconds() < 10 ? '0' + dateTime.getSeconds() : dateTime.getSeconds());

                return date + ' ' + time;
            },

            async reply(privateMessage) {
                if (this.isInbox) {
                    privateMessage.dateRead = privateMessage.dateRead === null ? this.dateFormatter(new Date()) : this.dateFormatter(new Date(privateMessage.dateRead));
                    await Api.privateMessages.editPrivateMessageAsync(privateMessage);
                }
                await this.$refs.formModal.showReply(privateMessage);
            },
        },

        mounted() {
            this.showInbox();
              var currentUser = JSON.parse(localStorage.getItem("currentUser"));
            UserStore.userRole = currentUser.userRole;
        }
    };
</script>