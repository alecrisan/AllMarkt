<template>
    <div class="modal fade">
        <div class="modal-dialog modal-dialog-centered modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 v-if="type === 'add'" class="modal-title">Add Private Message</h5>
                    <h5 v-if="type === 'edit'" class="modal-title">Edit Private Message</h5>
                    <button type="button" class="close" v-on:click="hide">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div v-if="isLoading" class="d-flex justify-content-center">
                        <div class="spinner-border text-info">
                            <span class="sr-only">Loading...</span>
                        </div>
                    </div>

                    <form v-else>
                        <div v-if="!isLoadingUsers && type === 'add' && privateMessage.receiver.id === null" class="d-flex justify-content-around">
                            <select id="selector" v-model="privateMessage.receiver.id" v-on:click="() => validateReceiver()" class="custom-select">
                                <option selected disabled>Receiver</option>
                                <option v-for="receiver in users" v-bind:value="receiver.id">{{receiver.displayName}}</option>
                            </select>
                        </div>
                        <div v-else class="form-group">
                            <label for="text">To</label>
                            <input v-model="privateMessage.receiver.displayName" disabled class="form-control" />
                        </div>
                        <div class="form-group">
                            <label for="title">Title</label>
                            <input v-model="privateMessage.title"
                                   v-on:input="validateTitle"
                                   v-on:blur="validateTitle"
                                   type="text"
                                   class="form-control"
                                   v-bind:class="titleValidClass"
                                   id="title"
                                   placeholder="Title" />
                            <div class="invalid-feedback">{{this.privateMessageError.title}}</div>
                        </div>
                        <div class="form-group">
                            <label for="text">Text</label>
                            <textarea v-model="privateMessage.text"
                                      v-on:input="validateText"
                                      v-on:blur="validateText"
                                      type="text"
                                      class="form-control"
                                      v-bind:class="textValidClass"
                                      id="text"
                                      placeholder="Text"
                                      rows="10" />
                            <div class="invalid-feedback">{{this.privateMessageError.text}}</div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button"
                            class="btn btn-primary"
                            v-on:click="saveAsync"
                            v-bind:disabled="isLoading || !privateMessageError.isValid">
                        Save
                    </button>
                    <button type="button"
                            class="btn btn-secondary"
                            v-on:click="hide"
                            v-bind:disabled="isLoading">
                        Cancel
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import $ from "jquery";
    import Api from "../../Api";

    export default {
        props: {
            "type": {
                default: null
            }
        },

        computed: {
            titleValidClass() {
                if (this.privateMessageError.title === null) return "";
                else if (this.privateMessageError.title === "") return "is-valid";
                else return "is-invalid";
            },

            textValidClass() {
                if (this.privateMessageError.text === null) return "";
                else if (this.privateMessageError.text === "") return "is-valid";
                else return "is-invalid";
            },

            receiverValidClass() {
                if (this.privateMessageError.receiver === null) return "";
                else if (this.privateMessage.receiver.id !== null) return "is-valid";
                else return "is-invalid";
            }
        },

        data() {
            return {
                users: null,
                isLoading: false,
                isLoadingUsers: true,
                privateMessage:
                {
                    id: 0,
                    title: null,
                    text: null,
                    dateSent: null,
                    dateRead: null,
                    sender:
                    {
                        id: null,
                        displayName: null
                    },
                    receiver:
                    {
                        id: null,
                        displayName: null
                    }
                },
                privateMessageError:
                {
                    title: null,
                    text: null,
                    receiver: null,
                    isValid: null
                }
            };
        },

        methods: {
            async loadUsers() {
                this.isLoadingUsers = true;
                this.users = (await Api.users.getAllAsync()).data;
                this.isLoadingUsers = false;
            },

            async showAdd() {
                await this.loadUsers();
                this.privateMessage = {
                    id: 0,
                    title: null,
                    text: null,
                    dateSent: null,
                    dateRead: null,
                    sender: JSON.parse(localStorage.getItem("currentUser")),
                    receiver:
                    {
                        id: null,
                        displayName: null
                    }
                };
                this.privateMessageError = {
                    title: null,
                    text: null,
                    receiver: null,
                    isValid: null
                };
                $(this.$el).modal("show");
            },

            async showEdit(privateMessage) {
                await this.loadUsers();
                this.privateMessage = privateMessage;
                this.privateMessage.dateSent = this.dateFormatter(new Date(privateMessage.dateSent));
                this.privateMessage.dateRead = privateMessage.dateRead === null ? null : this.dateFormatter(new Date(privateMessage.dateRead));
                this.privateMessageError = {
                    title: "",
                    text: null,
                    receiver: "",
                    isValid: false
                }
                $(this.$el).modal("show");
            },

            async showReply(privateMessage) {
                await this.loadUsers();
                this.privateMessage = {
                    id: 0,
                    title: "Re: " + privateMessage.title,
                    text: null,
                    dateSent: null,
                    dateRead: null,
                    sender: JSON.parse(localStorage.getItem("currentUser")),
                    receiver:
                    {
                        id: privateMessage.sender.id,
                        displayName: privateMessage.sender.displayName
                    }
                };

                this.privateMessageError = {
                    title: "",
                    text: null,
                    receiver: "",
                    isValid: false
                };

                $(this.$el).modal("show");
            },

            hide() {
                $(this.$el).modal("hide");
            },

            validateTitle() {
                this.privateMessageError.title = "";
                if (this.privateMessage.title === null || this.privateMessage.title.trim().length === 0)
                    this.privateMessageError.title = "Please provide a title for the message";
                else if (this.privateMessage.title.length > 20)
                    this.privateMessageError.title = "Title too long (max 20 characters)";

                this.checkValid();
            },

            validateText() {
                this.privateMessageError.text = "";

                if (this.privateMessage.text === null || this.privateMessage.text.trim().length === 0)
                    this.privateMessageError.text = "Please provide a message text";

                this.checkValid();
            },

            validateReceiver() {
                this.privateMessageError.receiver = "";
                if (this.privateMessage.receiver == null || this.privateMessage.receiver.id === null)
                    this.privateMessageError.receiver = "Please select a receiver for the message";
                else
                    this.privateMessage.receiver.displayName = document.getElementById("selector").options[document.getElementById("selector").selectedIndex].text;

                this.checkValid();
            },

            checkValid() {
                this.privateMessageError.isValid =
                    this.privateMessageError.title === "" &&
                    this.privateMessageError.text === "" &&
                    this.privateMessageError.receiver === "";
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

            async saveAsync() {
                try {
                    this.isLoading = true;
                    if (this.type === "add") {
                        this.privateMessage.dateSent = this.dateFormatter(new Date());
                        await Api.privateMessages.addPrivateMessageAsync(this.privateMessage);
                    }
                    else if (this.type === "edit") {
                        var temp = Object.assign({}, this.privateMessage);
                        await Api.privateMessages.editPrivateMessageAsync(temp);
                    }
                    this.hide();
                    this.$emit("submitted");
                }
                finally {
                    this.isLoading = false;
                }
            }
        }
    };
</script>


