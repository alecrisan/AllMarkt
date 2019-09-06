<template>
    <div class="modal fade">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Delete User</h5>
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
                    <span v-else>Are you sure you want to disable "{{user.displayName}}"?</span>
                </div>
                <div class="modal-footer">
                    <button type="button"
                            class="btn btn-danger"
                            v-on:click="deleteAsync"
                            v-bind:disabled="isLoading">
                        Yes
                    </button>
                    <button type="button"
                            class="btn btn-secondary"
                            v-on:click="hide"
                            v-bind:disabled="isLoading">
                        No
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
        data() {
            return {
                isLoading: false,
                user: {
                    id: 0,
                    displayName: "",
                }
            };
        },

        methods: {
            show(user) {
                this.user = user;
                $(this.$el).modal("show");
            },

            hide() {
                $(this.$el).modal("hide");
            },

            async deleteAsync() {
                try {
                    this.isLoading = true;
                    await Api.users.disableUserByIdAsync(this.user);
                    this.hide();
                    this.$emit("submitted");
                } finally {
                    this.isLoading = false;
                }
            }
        }
    };
</script>
