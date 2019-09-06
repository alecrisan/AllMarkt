<template>
    <layout>
    <div class="mx-5 my-3">
        <div class="d-flex align-items-center">
            <h2 class="flex-grow-1">Order History</h2>
        </div>
        <table class="table">
            <thead>
                <tr>
                    <th v-if="isCustomer" class="w-10">Seller</th>
                    <th v-else class="w-10">Buyer</th>
                    <th class="w-5">Price</th>
                    <th class="w-10">Status</th>
                    <th class="w-10">Actions</th>
                </tr>
            </thead>
            <tbody v-if="!isLoading">
                <tr v-for="order in orders" v-bind:key="order.id">
                    <td v-if="isCustomer">{{order.shopName}}</td>
                    <td v-else>{{order.customerName}}</td>
                    <td>{{order.totalPrice}}</td>
                    <td>{{convertStatus(order.orderStatus)}}</td>
                    <td>
                        <button v-if="isCustomer" class="btn btn-danger" v-on:click="()=>modifyStatus(order)">Cancel</button>
                        <button v-else class="btn btn-primary" v-on:click="()=>modifyStatus(order)">Modify Status</button>
                        <button class="btn btn-success" v-on:click="()=>showDetails(order)">Details</button>
                    </td>
                </tr>
            </tbody>
        </table>
        <div v-if="orders.length === 0" class="d-flex justify-content-center">You have no orders.</div>
        <div v-if="isLoading" class="d-flex justify-content-center">
            <div class="spinner-border text-info">
                <span class="sr-only">Loading...</span>
            </div>
        </div>

        <status-modal ref="statusModal" v-on:submitted="loadAsync" />
        <details-modal ref="detailsModal" v-on:submitted="loadAsync" />
    </div>
    </layout>
</template>
<script>

    import Api from "../../Api";
    import StatusModal from "./status-modal.vue";
    import DetailsModal from "./details-modal.vue";
    import UserStore from "../../UserStore";
    import Layout from "../Layout";
    export default {

        components: {
            StatusModal,
            DetailsModal,
            Layout
        },

        data() {
            return {
                isLoading: false,
                isCustomer: false,
                orders: []
            };
        },

        methods: {
            modifyStatus(order) {
                this.$refs.statusModal.show(order, this.isCustomer);
            },

            showDetails(order) {
                this.$refs.detailsModal.show(order, this.isCustomer);
            },

            async loadAsync() {
                try {
                    this.isLoading = true;
                    let result;

                    var user = await Api.users.getMyDataAsUserAsync();
                    var role = user.data.userRole;
                    if (role === "Customer") {
                        this.isCustomer = true;
                        var customer = await Api.customers.getCustomerByUserIdAsync(user.data.id);
                        result = await Api.orders.getCustomerOrdersAsync();
                    } else {
                        this.isCustomer = false;
                        var shop = await Api.shops.getShopByUserIdAsync(user.data.id)
                        result = await Api.orders.getShopOrdersAsync();
                    }
                    this.orders = result.data;
                } finally {
                    this.isLoading = false;
                }
            },

            convertStatus(orderStatus) {
                if (orderStatus === 0) {
                    return "Registered";
                }
                else
                    if (orderStatus === 1) {
                        return "Processed";
                    }
                    else
                        if (orderStatus === 2) {
                            return "SentToCourier";
                        }
                        else
                            if (orderStatus === 3) {
                                return "Delivered";
                            }
                            else
                                if (orderStatus === 4) {
                                    return "Cancelled";
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