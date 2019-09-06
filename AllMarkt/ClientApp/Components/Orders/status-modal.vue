<template>
    <div id="reply" class="modal fade">
        <div class="modal-dialog modal-dialog-centered modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5  v-if="!isCustomer" class="modal-title">Choose a new status for this order</h5>
                    <h5 v-else>Are you sure you want to cancel this order?</h5>
                    <button type="button" class="close" v-on:click="hide">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div v-if="!isCustomer" class="modal-body">
                    <div v-if="isLoading" class="d-flex justify-content-center">
                        <div class="spinner-border text-info">
                            <span class="sr-only">Loading...</span>
                        </div>
                    </div>

                    <form v-else>
                        <div v-if="order.orderStatus == 4">This order was cancelled.</div>
                        <div v-else class="form-group">
                            <select id="selector" class="custom-select" v-model="order.orderStatus" v-bind:key="order.orderStatus">
                                <option v-if="order.orderStatus == 0">Registered</option>
                                <option v-if="order.orderStatus == 0">Processed</option>
                                <option v-if="order.orderStatus <= 1">SentToCourier</option>
                                <option v-if="order.orderStatus <= 2">Delivered</option>
                                <option>Cancelled</option>
                            </select>
                        </div>
                    </form>
                </div>
                <div v-if="order.orderStatus != 4" class="modal-footer">
                    <button type="button"
                            class="btn btn-primary"
                            v-on:click="saveAsync">
                        Save
                    </button>

                    <button type="button"
                            class="btn btn-secondary"
                            v-on:click="hide">
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

        data() {
            return {
                isLoading: false,
                order: {
                    id: 0,
                    shopId: 0,
                    shopName: "",
                    customerId: 0,
                    customerName: "",
                    deliveryPhoneNumber: "",
                    deliveryAddress: "",
                    totalPrice: 0,
                    additionalNotes: "",
                    orderStatus: "",
                    awb: "",
                    orderItems: null
                },
                isCustomer: false
            };
        },

        methods: {
            async show(order, isCustomer) {
                this.order = order;
                this.isCustomer = isCustomer;
                $(this.$el).modal("show");
            },

            hide() {
                $(this.$el).modal("hide");
            },

            async saveAsync() {
                if (this.isCustomer)
                    this.order.orderStatus = 4;
                await Api.orders.editAsync(this.order);
                this.hide();
                this.$emit("submitted");
            }
        }
    };
</script>