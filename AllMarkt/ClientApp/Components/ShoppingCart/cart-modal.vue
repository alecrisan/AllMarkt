<template>
    <div class="modal fade bd-example-modal-xl" id="cart-modal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true" v-on:click="computePriceOfAllOrders;changeNr()">
        <div class="modal-dialog modal-dialog-centered modal-xl" role="document">
            <div style="height:900px ; overflow-y:scroll " class="modal-content">
                <div class="modal-header">
                    <h3 class="modal-title">Shopping Cart</h3>
                    <button type="button" class="close" aria-label="Close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <div id="carouselExampleControls" class="carousel slide" data-ride="carousel" data-interval="false">
                        <div class="carousel-inner">
                            <div class="carousel-item active">
                                <div v-if="(this.shopCart == null || this.shopCart.length==0)">
                                    <h1>
                                        Your Cart is empty!
                                    </h1>
                                </div>
                                <div v-else>
                                    <div id="accordion2">
                                        <div class="card" v-for="order in shopCart">
                                            <div class="card-header" id="headingOne2">
                                                <div data-toggle="collapse" :data-target="'#modalCollapse'+order.shop.id" aria-expanded="false" :aria-controls="'#modalCollapse'+order.shop.id">
                                                    <h5 class="mb-0">
                                                        {{order.shop.userDisplayName}}
                                                    </h5>
                                                </div>
                                            </div>
                                            <div :id="'modalCollapse'+order.shop.id" class="collapse show" aria-labelledby="headingOne2" data-parent="#accordion2">
                                                <div class="card-body">
                                                    <table class="table table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>Name</th>
                                                                <th>Description</th>
                                                                <th>Price</th>
                                                                <th>Quantity</th>
                                                                <th>Actions</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr v-for="item in order.orderItems" class="border border-secondary" v-bind:key="item.product.id">
                                                                <td>{{item.product.name}}</td>
                                                                <td>{{item.product.description.length<100 ? item.product.description : item.product.description.substr(0,97)+"..."}}</td>
                                                                <td>{{item.product.price}}</td>
                                                                <td>
                                                                    <input :id="'CartQuantity'+item.product.id" type="number" min="1" placeholder="1" v-model="item.quantity" style="width:50px" v-on:change="()=>changeTotalPrice(order)" />
                                                                </td>
                                                                <td>
                                                                    <button class="btn btn-primary" v-on:click="()=>removeItemFromCart(item, order)">Remove from Cart</button>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>

                                                </div>
                                            </div>
                                            <div class="d-flex justify-content-end align-items-center">
                                                <div class="d-flex flex-row p-3">
                                                    Total price:&nbsp&nbsp
                                                    {{order.totalPrice}}
                                                </div>
                                                <div class=" p-1">
                                                    <button class="btn btn-primary" v-on:click="removeOrder(order)">Remove order</button>
                                                </div>
                                                <div class="p-3">
                                                    <button class="btn btn-primary" v-on:click="checkoutOrder(order)">Checkout order</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                </div>
                            </div>

                            <div class="carousel-item ">
                                <div class="d-flex flex-row justify-content-around mt-5">
                                    <div class="card bg-light mb-3 w-100" style="max-width: 28rem;">
                                        <div class="card-header">Billing Details</div>
                                        <div class="card-body d-flex flex-column">
                                            <label>Customer Name:</label>
                                            <input type="text" v-model="user.name" />

                                            <label>Billing Phone Number:</label>
                                            <input type="tel" v-model="user.phoneNumber" />

                                            <label>Billing Address:</label>
                                            <input type="text" v-model="user.address" />
                                        </div>
                                    </div>
                                    <div class="card bg-light mb-3 w-100" style="max-width: 28rem;">
                                        <div class="card-header">Delivery Details</div>
                                        <div class="card-body d-flex flex-column">
                                            <!--<label>Delivery Phone Number:</label>
    <input type="tel" v-model="billingDetails.deliveryPhoneNumber" />-->
                                            <div class="form-group">
                                                <label for="phoneNumber">Phone Number: </label>
                                                <input v-model="billingDetails.deliveryPhoneNumber"
                                                       type="text"
                                                       v-on:input="validatePhoneNumber"
                                                       v-on:blur="validatePhoneNumber"
                                                       class="form-control"
                                                       v-bind:class="isValidClass.phone"
                                                       id="phoneNumber" />
                                                <div class="invalid-feedback">{{this.errorMessages.phoneError}}</div>
                                            </div>


                                            <label>Delivery Address:</label>
                                            <input v-model="billingDetails.deliveryAddress"
                                                   v-on:input="validateAddress"
                                                   v-on:blur="validateAddress"
                                                   type="text"
                                                   class="form-control"
                                                   v-bind:class="isValidClass.address"
                                                   id="address" />
                                            <div class="invalid-feedback">{{this.errorMessages.addressError}}</div>

                                            <label>AWB:</label>
                                            <input type="text" v-model="billingDetails.awb" readonly />
                                        </div>
                                    </div>

                                </div>

                                <div class="card bg-light mb-3 w-80 ml-5 mr-5">
                                    <div class="card-header d-flex flex-fill ">Additional Notes</div>
                                    <div class="card-body d-flex flex-fill flex-column">
                                        <textarea v-model="billingDetails.additionalNotes"
                                                  v-on:input="validateNotes"
                                                  v-on:blur="validateNotes"
                                                  type="text"
                                                  class="form-control"
                                                  v-bind:class="isValidClass.notes"
                                                  id="notes" />
                                        <div class="invalid-feedback">{{this.errorMessages.notesError}}</div>
                                    </div>
                                    
                                </div>

                            </div>
                        </div>
                    </div>



                </div>
                <div class="modal-footer">
                    <div id="carouselExampleControls" class="carousel slide" data-ride="carousel" data-interval="false">
                        <div class="carousel-inner">
                            <div class="carousel-item active">
                                <div class="d-flex flex-row align-items-center">
                                    <div v-if="priceOfAllOrders!=0" class="p-2">
                                        Price of all orders: {{priceOfAllOrders}}
                                    </div>
                                    <div class="p-1">
                                        <button type="button" class="btn btn-secondary " data-dismiss="modal">Close</button>
                                    </div>
                                    <div class="p-3">
                                        <button type="button" class="btn btn-primary " v-bind:class="isDisabled" v-on:click="checkoutAllOrders">Checkout All Orders</button>
                                    </div>
                                </div>
                            </div>

                            <div class="carousel-item ">
                                <div class="d-flex flex-row align-items-center">
                                    <div v-if="priceOfAllOrders!=0" class="p-2">
                                        Price of order: {{priceOfOrdersToSend}}
                                    </div>
                                    <div class="p-1">
                                        <button class="btn btn-secondary" v-on:click="goBack">
                                            Back
                                        </button>
                                    </div>
                                    <div class="p-3">
                                        <button class="btn btn-primary" v-on:click="sendOrder">
                                            Send Order
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
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
                billingDetails: {
                    deliveryPhoneNumber: "",
                    deliveryAddress: "",
                    awb: "AWBs",
                    additionalNotes: "",
                },
                user: {
                    id: 1,
                    name: "placeholder",
                    userRole: "Customer",
                    phoneNumber: "0123456789",
                    address: "adr"

                },
                priceOfAllOrders: 0,
                priceOfOrdersToSend: 0,
                shopCart: null,
                errorMessages: {
                    phoneError: null,
                    addressError: null,
                    notesError: null
                },
                isValidClass: {
                    phone: "",
                    address: "",
                    notes: ""
                }
            }

        },
        computed: {
            isDisabled() {
                if (this.shopCart == null || this.shopCart.length == 0) {
                    return "disabled";
                } else {
                    return "";
                }
            },
            computePriceOfAllOrders() {
                this.priceOfAllOrders = 0;
                for (var i = 0; i < this.shopCart.length; i++) {
                    this.priceOfAllOrders = (parseFloat(this.priceOfAllOrders) + parseFloat(this.shopCart[i].totalPrice));
                }
            }
        },

        methods: {
            show(cart, user) {
                $(this.$el).modal("show");
                this.shopCart = cart;
                this.user = user;
                 this.priceOfAllOrders = 0;
                for (var i = 0; i < this.shopCart.length; i++) {
                    this.priceOfAllOrders = parseFloat(this.priceOfAllOrders) + parseFloat(this.shopCart[i].totalPrice);
                }
            },
            changeNr() {
                var totalQuantity = 0;
                for (var i = 0; i < this.shopCart.length; i++) {
                    for (var j = 0; j < this.shopCart[i].orderItems.length; j++) {
                        totalQuantity = parseInt(totalQuantity) + parseInt(this.shopCart[i].orderItems[j].quantity);
                    }
                }
                this.$emit("changeNr", totalQuantity);
            },
            changeTotalPrice(order) {
                order.totalPrice = 0;
                for (var i = 0; i < order.orderItems.length; i++) {
                    order.orderItems[i].price = (order.orderItems[i].product.price * parseFloat(order.orderItems[i].quantity));
                    order.totalPrice = order.totalPrice + order.orderItems[i].price;
                }
            },
            removeItemFromCart(item, order) {
                for (var i = 0; i < order.orderItems.length; i++) {
                    if (order.orderItems[i].id === item.id)
                        order.orderItems.splice(i, 1);
                }
                this.changeTotalPrice(order);
                if (order.orderItems.length == 0) {
                    this.removeOrder(order);
                }

            },
            removeOrder(order) {
                for (var i = 0; i < this.shopCart.length; i++) {
                    if (this.shopCart[i].shop.id == order.shop.id) {
                        this.shopCart.splice(i, 1);
                    }
                }
            },
            checkoutOrder(order) {
                $('.carousel').carousel('next');
                order.toSend = "toSend";
                this.priceOfOrdersToSend += order.totalPrice;
            },
            goBack() {
                for (var i = 0; i < this.shopCart.length; i++) {
                    this.shopCart[i].toSend = null;
                }
                this.priceOfOrdersToSend = 0;
                $('.carousel').carousel('prev');
                this.priceOfAllOrders = 0;
                for (var i = 0; i < this.shopCart.length; i++) {
                    this.priceOfAllOrders = parseInt(this.priceOfAllOrders) + parseInt(this.shopCart[i].totalPrice);
                }
            },
            checkoutAllOrders() {
                for (var i = 0; i < this.shopCart.length; i++) {
                    this.checkoutOrder(this.shopCart[i]);
                }
            },
            async sendOrder() {
                for (var i = 0; i < this.shopCart.length; i++) {
                    if (this.shopCart[i].toSend != null) {
                        var ord = this.mapOrderToVM(this.shopCart[i]);
                        await Api.orders.addAsync(ord);
                        this.removeOrder(this.shopCart[i]);
                        i--;
                    }
                }
                alert("Order has been sent!");
                this.goBack();
            },
            mapOrderToVM(order) {
                var orderVM = {
                    shopId: order.shop.id,
                    shopName: order.shop.userDisplayName,
                    customerId: 1,
                    customerName: this.user.name,
                    deliveryPhoneNumber: this.billingDetails.deliveryPhoneNumber,
                    deliveryAddress: this.billingDetails.deliveryAddress,
                    totalPrice: this.priceOfOrdersToSend,
                    additionalNotes: this.billingDetails.additionalNotes,
                    awb: this.billingDetails.awb,
                    orderItems: []
                };
                for (var i = 0; i < order.orderItems.length; i++) {
                    var orderItemVM = {
                        name: order.orderItems[i].product.name,
                        amount: order.orderItems[i].quantity,
                        price: parseFloat(order.orderItems[i].product.price) * parseFloat(order.orderItems[i].quantity)
                    };
                    orderVM.orderItems.push(orderItemVM);
                }
                return orderVM;
            },
            validatePhoneNumber() {
                this.errorMessages.phoneError = null;
                if (this.billingDetails.deliveryPhoneNumber[0] != '0')
                    this.errorMessages.phoneError = "Phone number must start with a 0";
                if (this.billingDetails.deliveryPhoneNumber.length != 10)
                    this.errorMessages.phoneError = "Phone number must contain 10 digits";

                if (this.errorMessages.phoneError === null) {
                    this.isValidClass.phone = "is-valid";
                } else {
                    this.isValidClass.phone = "is-invalid";
                }
            },
            validateAddress() {
                this.errorMessages.addressError = null;
                if (this.billingDetails.deliveryAddress.trim().length === 0) {
                    this.errorMessages.addressError = "Please enter an address";
                }
                if (this.billingDetails.deliveryAddress.length > 255)
                    this.errorMessages.addressError = "Address too long (max 255 characters)";

                if (this.errorMessages.addressError === null) {
                    this.isValidClass.address = "is-valid";
                } else {
                    this.isValidClass.address = "is-invalid";
                }
            },
            validateNotes() {
                this.errorMessages.notesError = null;
                if (this.billingDetails.additionalNotes.length > 255) {
                    this.errorMessages.notesError = "Additional Notes too long (max 255 characters)";
                }

                if (this.errorMessages.notesError === null) {
                    this.isValidClass.notes = "is-valid";
                } else {
                    this.isValidClass.notes = "is-invalid";
                }
            },
        }
    }
</script>
