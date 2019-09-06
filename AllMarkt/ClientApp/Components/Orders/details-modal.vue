<template>
    <div class="modal fade">
        <div class="modal-dialog modal-dialog-centered modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Order Details</h5>
                    <button type="button" class="close" v-on:click="hide">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>

                <div class="modal-body">
                    <div class="form-row">
                        <div class="form-group, col">
                            <label for="shop">Shop Name</label>
                            <input disabled v-model="order.shopName"
                                   type="text"
                                   id="shop"
                                   class="form-control" />
                        </div>

                        <div class="form-group, col">
                            <label for="customer">Customer Name</label>
                            <input disabled v-model="order.customerName"
                                   type="text"
                                   id="customer"
                                   class="form-control" />
                        </div>
                    </div>

                    <br />

                    <div class="form-row">
                        <div class="form-group, col">
                            <label for="email">Shop Email</label>
                            <input v-if="isCustomer" disabled v-model="email"
                                   type="text"
                                   id="email2"
                                   class="form-control" />
                            <input v-else disabled v-model="user.email"
                                   type="text"
                                   id="email"
                                   class="form-control" />
                        </div>

                        <div class="form-group, col">
                            <label for="email">Customer Email</label>
                            <input v-if="!isCustomer" disabled v-model="email"
                                   type="text"
                                   id="email2"
                                   class="form-control" />
                            <input v-else disabled v-model="user.email"
                                   type="text"
                                   id="email"
                                   class="form-control" />
                        </div>
                    </div>

                    <br />

                    <div class="form-row">
                        <div class="form-group, col">
                            <label for="shopPhoneNumber">Shop Phone Number</label>
                            <input disabled v-model="shop.phoneNumber"
                                   type="text"
                                   id="shopPhoneNumber"
                                   class="form-control" />
                        </div>

                        <div class="form-group, col">
                            <label for="customerPhoneNumber">Customer Phone Number</label>
                            <input disabled v-model="customer.phoneNumber"
                                   type="text"
                                   id="customerPhoneNumber"
                                   class="form-control" />
                        </div>
                    </div>

                    <br />

                    <div class="form-row">
                        <div class="form-group, col">
                            <label for="shopAdress">Shop Address</label>
                            <input disabled v-model="shop.address"
                                   type="text"
                                   id="address"
                                   class="form-control" />
                        </div>

                        <div class="form-group, col">
                            <label for="address">Customer Address</label>
                            <input disabled v-model="customer.address"
                                   type="text"
                                   id="customerAddress"
                                   class="form-control" />
                        </div>
                    </div>

                    <br />

                    <div class="form-group">
                        <label for="deliveryPhoneNumber">Delivery Phone Number</label>
                        <input disabled v-model="order.deliveryPhoneNumber"
                               type="text"
                               id="deliveryPhoneNumber"
                               class="form-control" />
                    </div>

                    <div class="form-group">
                        <label for="deliveryAddress">Delivery Address</label>
                        <input disabled v-model="order.deliveryAddress"
                               type="text"
                               id="deliveryAddress"
                               class="form-control" />
                    </div>

                    <div class="form-group">
                        <label for="additionalNotes">Additional Notes</label>
                        <input disabled v-model="order.additionalNotes"
                               type="text"
                               id="additionalNotes"
                               class="form-control" />
                    </div>

                    <div class="form-group">
                        <label for="awb">AWB</label>
                        <input disabled v-model="order.awb"
                               type="text"
                               id="awb"
                               class="form-control" />
                    </div>

                    <div class="form-group">
                        <div>
                            <button class="btn btn-success" data-toggle="collapse" :href="'#list'+order.id" role="button" aria-expanded="false" aria-controls="itemlist">Items</button>
                        </div>
                        <div :id="'list'+order.id" class="panel-collapse collapse">
                                    <table class="table">
                                        <thead>
                                            <tr>
                                                <th class="w-20">Name</th>
                                                <th class="w-20">Amount</th>
                                                <th class="w-20">Price</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr v-for="item in order.orderItems" v-bind:key="item.id">
                                                <td>{{item.name}}</td>
                                                <td>{{item.amount}}</td>
                                                <td>{{item.price}}</td>
                                            </tr>
                                        </tbody>
                                    </table>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="totalPrice">Total Price</label>
                        <input disabled v-model="order.totalPrice"
                               type="text"
                               id="totalPrice"
                               class="form-control" />
                    </div>

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
                isCustomer: false,
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
                    awb: "",
                    orderItems: null
                },
                user: {
                    id: 0,
                    email: "",
                    password: "",
                    displayName: "",
                    userRole: "",
                },
                shop: {
                    id: 0,
                    userId: 0,
                    userDisplayName: "",
                    phoneNumber: "",
                    address: "",
                    iban: ""
                },
                customer: {
                    id: 0,
                    userId: 0,
                    userDisplayName: "",
                    phoneNumber: "",
                    address: "",
                },
                email: ""
            };
        },

        methods: {
            async show(order, isCustomer) {
                try {
                    this.isLoading = true;
                    this.order = order;

                    var user = await Api.users.getMyDataAsUserAsync();
                    this.user = user.data;


                    if (user.data.userRole === "Shop") {
                        var shop = await Api.shops.getShopByUserIdAsync(user.data.id);
                        this.shop = shop.data;

                        var customer = await Api.customers.getCustomerByIdAsync(order.customerId);
                        this.customer = customer.data;
                        var u = await Api.users.getByIdAsync(this.customer.userId);
                        this.email = u.data.email;
                    }
                    else {
                        var shop = await Api.shops.getShopByIdAsync(order.shopId);
                        this.shop = shop.data;

                        var customer = await Api.customers.getCustomerByUserIdAsync(user.data.id);
                        this.customer = customer.data;

                        var u = await Api.users.getByIdAsync(this.shop.userId);
                        this.email = u.data.email;
                    }

                    this.isCustomer = isCustomer;
                    $(this.$el).modal("show");
                } finally {
                    this.isLoading = false;
                }
            },

            hide() {
                $(this.$el).modal("hide");
            }
        }
    };
</script>