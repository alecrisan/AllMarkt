<template>
    <layout>
        <div class="d-flex align-items-center">
            <h2 class="flex-grow-1">Orders</h2>
            <button class="btn btn-primary" v-on:click="()=>showForm(null)">Add</button>
        </div>
        <table class="table">
            <thead>
                <tr>
                    <th class="w-10">Seller</th>
                    <th class="w-10">Buyer</th>
                    <th class="w-25">Address</th>
                    <th class="w-5">Price</th>
                    <th class="w-30">Notes</th>
                    <th class="w-10">Items</th>
                    <th class="w-10">Actions</th>
                </tr>
            </thead>
            <tbody v-if="!isLoading">
                <tr v-for="order in orders" v-bind:key="order.id">
                    <td>{{order.shopName}}</td>
                    <td>{{order.customerName}}</td>
                    <td>{{order.deliveryAddress}}</td>
                    <td>{{order.totalPrice}}</td>
                    <td>{{order.additionalNotes}}</td>
                    <td>
                        <div>
                            <button class="btn btn-success" data-toggle="collapse" :href="'#list'+order.id" role="button" aria-expanded="false" aria-controls="itemlist">Items</button>
                        </div>
                        <div :id="'list'+order.id" class="panel-collapse collapse">
                            <ul class="list-group" v-for="item in order.orderItems" v-bind:key="item.id">
                                <li class="list-group-item">{{item.name}}</li>
                            </ul>
                        </div>
                    </td>
                    <td>
                        <button type="button" class="btn btn-primary" v-on:click="()=>showForm(order)"> Edit</button>
                        <button class="btn btn-danger" v-on:click="()=>showDelete(order)">Delete</button>
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
                orders: [],
                test: null
            };
        },

        methods: {
            showForm(order) {
                this.$refs.formModal.show(order);
            },
            showDelete(order) {
                this.$refs.deleteModal.show(order);
            },

            async loadAsync() {
                try {
                    this.isLoading = true;
                    let result = await Api.orders.getAllAsync();
                    this.orders = result.data;
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