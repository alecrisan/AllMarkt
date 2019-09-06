<template>
    <div id="reply" class="modal fade">
        <div class="modal-dialog modal-dialog-centered modal-lg">
            <div class="modal-content">
                <div class="modal-header">
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
                        <div class="form-group">
                            <label for="text">From</label>
                            <input v-model="privateMessage.sender.displayName" disabled class="form-control" />
                        </div>
                        <div class="form-group">
                            <label for="text">Subject</label>
                            <input v-model="privateMessage.title" disabled class="form-control" />
                        </div>
                        <div class="form-group">
                            <label for="text">Message</label>
                            <textarea v-model="privateMessage.text" disabled class="form-control" rows="10" />
                        </div>
                        <div class="form-group">
                            <label for="text">Date</label>
                            <input v-model="privateMessage.dateSent" disabled class="form-control" />
                        </div>
                    </form>
                </div>
            </div>
        </div>
        <form-modal id="new" :type=formType ref="formModal" />
    </div>
</template>
<script>
    import $ from "jquery";
    import Api from "../../Api";
    import FormModal from "./form-modal.vue";

    export default {
        components: {
            FormModal
        },

        data() {
            return {
                isLoading: false,
                isInbox: false,
                formType: "add",
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
                }
            };
        },

        methods: {
            async show(privateMessage, type) {
                this.isInbox = type;
                this.privateMessage = privateMessage;
                this.privateMessage.dateSent = this.dateFormatter(new Date(privateMessage.dateSent));

                if (this.isInbox) {
                    this.privateMessage.dateRead = privateMessage.dateRead === null ? this.dateFormatter(new Date()) : this.dateFormatter(new Date(privateMessage.dateRead));
                    await Api.privateMessages.editPrivateMessageAsync(this.privateMessage);
                }
                $(this.$el).modal("show");
            },

            hide() {
                $(this.$el).modal("hide");
            },

            dateFormatter(dateTime) {
                var date = dateTime.getFullYear() + '-' +
                    ((dateTime.getMonth() + 1) < 10 ? '0' + (dateTime.getMonth() + 1) : (dateTime.getMonth() + 1)) + '-' +
                    (dateTime.getDate() < 10 ? '0' + dateTime.getDate() : dateTime.getDate());

                var time = (dateTime.getHours() < 10 ? '0' + dateTime.getHours() : dateTime.getHours()) + ":" +
                    (dateTime.getMinutes() < 10 ? '0' + dateTime.getMinutes() : dateTime.getMinutes()) + ":" +
                    (dateTime.getSeconds() < 10 ? '0' + dateTime.getSeconds() : dateTime.getSeconds());

                return date + ' ' + time;
            }
        }
    };
</script>

