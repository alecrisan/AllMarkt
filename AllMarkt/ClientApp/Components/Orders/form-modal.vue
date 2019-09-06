<template>
    <div class="modal fade">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 v-if="order.id===0" class="modal-title">Add Order</h5>
                    <h5 v-else class="modal-title">Edit Order</h5>
                    <button type="button" class="close" v-on:click="hide">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="shopId">Shop</label>
                        <select id="shopSelect" v-model="order.shopId" v-on:click="validateShopSelect" class="custom-select" v-bind:class="isValidClass.shopId">
                            <option selected disabled value="null">Shop</option>
                            <option v-for="shop in shops" v-bind:value="shop.id">{{shop.userDisplayName}}</option>
                        </select>
                        <div class="invalid-feedback">{{this.errorMessages.shopError}}</div>
                    </div>
                    <div class="form-group">
                        <label for="customerId">Customer</label>
                        <select id="customerSelect" v-model="order.customerId" v-on:click="validateCustomerSelect" class="custom-select" v-bind:class="isValidClass.customerId">
                            <option selected disabled value="null">Customer</option>
                            <option v-for="cust in customers" v-bind:value="cust.id">{{cust.userDisplayName}}</option>
                        </select>
                        <div class="invalid-feedback">{{this.errorMessages.customerError}}</div>
                    </div>
                    <div class="form-group">
                        <label for="phoneNumber">Phone Number</label>
                        <input v-model="order.deliveryPhoneNumber"
                               type="text"
                               v-on:input="validatePhoneNumber"
                               v-on:blur="validatePhoneNumber"
                               class="form-control"
                               v-bind:class="isValidClass.phone"
                               id="phoneNumber" />
                        <div class="invalid-feedback">{{this.errorMessages.phoneError}}</div>
                    </div>
                    <div class="form-group">
                        <label for="address">Delivery Address</label>
                        <input v-model="order.deliveryAddress"
                               v-on:input="validateAddress"
                               v-on:blur="validateAddress"
                               type="text"
                               class="form-control"
                               v-bind:class="isValidClass.address"
                               id="address" />
                        <div class="invalid-feedback">{{this.errorMessages.addressError}}</div>
                    </div>
                    <div class="form-group">
                        <label for="price">Price</label>
                        <input v-model="order.totalPrice"
                               type="number"
                               class="form-control"
                               v-bind:class="isValidClass.price"
                               id="price" />
                    </div>
                    <div class="form-group">
                        <label for="awb">AWB</label>
                        <input v-model="order.awb"
                               type="text"
                               v-on:input="validateAWB"
                               v-on:blur="validateAWB"
                               class="form-control"
                               v-bind:class="isValidClass.awb"
                               id="awb" />
                        <div class="invalid-feedback">{{this.errorMessages.awbError}}</div>
                    </div>
                    <div class="form-group">
                        <label for="notes">Additional Notes</label>
                        <textarea v-model="order.additionalNotes"
                                  v-on:input="validateNotes"
                                  v-on:blur="validateNotes"
                                  type="text"
                                  class="form-control"
                                  v-bind:class="isValidClass.notes"
                                  id="notes" />
                        <div class="invalid-feedback">{{this.errorMessages.notesError}}</div>
                    </div>
                    <div class="form-group ">
                        <label for="items">Items</label>

                        <br>
                        <div class="input-group" id="dynamic-fields" v-bind:class="isValidClass.items">

                            <div id="fields" class="form-check-inline">
                                <input type="text" class="form-control" id="item" placeholder="Item Name" />
                                <input type="number" class="form-control" id="amount" placeholder="Item Amount" />
                                <br />
                                <br />
                            </div>
                        </div>
                        <div><font color="red">{{this.errorMessages.itemError}}</font></div>

                        <div>
                            <button type="button" class="btn btn-danger" v-on:click="removeItem">&times;</button>
                            <button type="button" class="btn btn-primary" v-on:click="addItem">+</button>
                        </div>

                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button"
                            class="btn btn-primary" v-on:click="saveAsync" v-bind:disabled="isLoading">
                        Save
                    </button>
                    <button type="button"
                            class="btn btn-secondary" v-on:click="hide">
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
                customers: [],
                shops: [],
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
                errorMessages: {
                    shopError: null,
                    customerError: null,
                    phoneError: null,
                    addressError: null,
                    notesError: null,
                    awbError: null,
                    itemError: null,
                },
                isValidClass: {
                    shopId: "",
                    customerId: "",
                    phone: "",
                    address: "",
                    notes: "",
                    awb: "",
                    items: ""
                }
            }
        },
        methods: {
            async show(ord) {
                try {
                    this.isLoading = true;
                    let customersResult = await Api.customers.getAllCustomersAsync();
                    let shopsResult = await Api.shops.getAllShopsAsync();
                    this.customers = customersResult.data;
                    this.shops = shopsResult.data;
                }
                finally {
                    this.isLoading = false;
                }

                this.removeAll();
                if (ord != null) {
                    this.order = {
                        id: ord.id,
                        shopId: ord.shopId,
                        shopName: ord.shopName,
                        customerId: ord.customerId,
                        customerName: ord.customerName,
                        deliveryPhoneNumber: ord.deliveryPhoneNumber,
                        deliveryAddress: ord.deliveryAddress,
                        totalPrice: ord.totalPrice,
                        additionalNotes: ord.additionalNotes,
                        awb: ord.awb,
                        orderItems: ord.orderItems
                    };
                    this.addExistingItems(ord.orderItems);
                }
                else {
                    this.order = {
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
                    };
                }
                this.errorMessages = {
                    shopError: null,
                    customerError: null,
                    phoneError: null,
                    addressError: null,
                    notesError: null,
                    awbError: null,
                    itemError: null,
                };
                this.isValidClass = {
                    shopId: "",
                    customerId: "",
                    phone: "",
                    address: "",
                    notes: "",
                    awb: "",
                    items: ""
                };

                $(this.$el).modal("show");
            },
            hide() {
                $(this.$el).modal("hide");
            },

            addExistingItems(list) {
                var container = document.getElementById("dynamic-fields");
                var first_field_group = document.getElementById("fields");
                first_field_group.children.item(0).value = list[0].name;
                first_field_group.children.item(1).value = list[0].amount;
                for (var i = 1; i < list.length; i++) {
                    var new_field_group = document.getElementById("fields").cloneNode(true);
                    new_field_group.children.item(0).value = list[i].name;
                    new_field_group.children.item(1).value = list[i].amount;
                    container.append(new_field_group);
                }
            },
            addItem() {
                var container = document.getElementById("dynamic-fields");
                var new_field_group = document.getElementById("fields").cloneNode(true);
                new_field_group.children.item(0).value = "";
                new_field_group.children.item(1).value = "";
                container.append(new_field_group);
            },
            removeItem() {
                var container = document.getElementById("dynamic-fields");
                var field_group_to_delete = container.lastChild;
                if (container.children.length > 1) {
                    field_group_to_delete.remove();
                }
            },
            removeAll() {
                var container = document.getElementById("dynamic-fields");
                while (container.children.length > 1) {
                    var field_group_to_delete = container.lastChild;
                    field_group_to_delete.remove();
                }
                var field_group = container.lastChild;
                field_group.children.item(0).value = "";
                field_group.children.item(1).value = "";

            },

            async saveAsync() {
                try {

                    this.isLoading = true;
                    var orderItemsList = [];

                    var list = document.getElementById("dynamic-fields").children;
                    for (var i = 0; i < list.length; i++) {
                        var itemName = list.item(i).children.item(0).value;
                        var itemAmount = list.item(i).children.item(1).value;
                        var item = { name: itemName, amount: itemAmount };
                        this.validateItem(item);
                        orderItemsList.push(item);
                    }
                    console.log(orderItemsList);
                    if (this.isValid()) {
                        this.order.orderItems = orderItemsList;
                        console.log(orderItemsList);
                        if (this.order.id == 0) {
                            console.log(orderItemsList);
                            await Api.orders.addAsync(this.order);
                        }
                        else {
                            await Api.orders.editAsync(this.order);
                        }
                        this.hide();
                        this.$emit("submitted");
                    }
                } finally {
                    this.isLoading = false;
                }
            },

            validateItem(item) {
                this.errorMessages.itemError = null;
                if (item.amount === 0) {
                    this.errorMessages.itemError = "Please enter item amount";
                }
                if (item.name === "") {
                    this.errorMessages.itemError = "Please enter item name";
                }
                if (this.errorMessages.itemError === null) {
                    this.isValidClass.items = "is-valid";
                } else {
                    this.isValidClass.items = "is-invalid";
                }

            },
            validateShopSelect() {
                this.errorMessages.shopError = null;
                if (this.order.shopId == 0) {
                    this.errorMessages.shopError = "Please select a shop";
                }

                if (this.errorMessages.shopError === null) {
                    this.isValidClass.shopId = "is-valid";
                    this.order.shopName = document.getElementById("shopSelect").options[document.getElementById("shopSelect").selectedIndex].text;
                } else {
                    this.isValidClass.shopId = "is-invalid";
                }
            },
            validateCustomerSelect() {
                this.errorMessages.customerError = null;
                if (this.order.customerId == 0) {
                    this.errorMessages.customerError = "Please select a customer";
                }

                if (this.errorMessages.customerError === null) {
                    this.isValidClass.customerId = "is-valid";
                    this.order.customerName = document.getElementById("customerSelect").options[document.getElementById("customerSelect").selectedIndex].text;
                } else {
                    this.isValidClass.customerId = "is-invalid";
                }
            },
            validatePhoneNumber() {
                this.errorMessages.phoneError = null;
                if (this.order.deliveryPhoneNumber[0] != '0')
                    this.errorMessages.phoneError = "Phone number must start with a 0";
                if (this.order.deliveryPhoneNumber.length != 10)
                    this.errorMessages.phoneError = "Phone number must contain 10 digits";

                if (this.errorMessages.phoneError === null) {
                    this.isValidClass.phone = "is-valid";
                } else {
                    this.isValidClass.phone = "is-invalid";
                }
            },
            validateAddress() {
                this.errorMessages.addressError = null;
                if (this.order.deliveryAddress.trim().length === 0) {
                    this.errorMessages.addressError = "Please enter an address";
                }
                if (this.order.deliveryAddress.length > 255)
                    this.errorMessages.addressError = "Address too long (max 255 characters)";

                if (this.errorMessages.addressError === null) {
                    this.isValidClass.address = "is-valid";
                } else {
                    this.isValidClass.address = "is-invalid";
                }
            },
            validateNotes() {
                this.errorMessages.notesError = null;
                if (this.order.additionalNotes.length > 255) {
                    this.errorMessages.notesError = "Additional Notes too long (max 255 characters)";
                }

                if (this.errorMessages.notesError === null) {
                    this.isValidClass.notes = "is-valid";
                } else {
                    this.isValidClass.notes = "is-invalid";
                }
            },
            validateAWB() {
                this.errorMessages.awbError = null;
                if (this.order.awb.trim().length === 0) {
                    this.errorMessages.awbError = "Please enter an AWB";
                }

                if (this.errorMessages.awbError === null) {
                    this.isValidClass.awb = "is-valid";
                } else {
                    this.isValidClass.awb = "is-invalid";
                }
            },

            isValid() {
                var valid = true;
                if (this.isValidClass.shopId != null && this.order.shopId === 0)
                    valid = false, console.log("1");
                if (this.errorMessages.customerId != null && this.order.customerId === 0)
                    valid = false, console.log("2");
                if (this.errorMessages.phone != null && this.order.deliveryPhoneNumber === "")
                     valid =   false, console.log("3");
                if (this.errorMessages.address != null && this.order.deliveryAddress === "")
                     valid =   false, console.log("4");
                if (this.errorMessages.notes != null && this.order.additionalNotes === "")
                    valid = false, console.log("5");
                if (this.errorMessages.awb != null && this.order.awb === "")
                    valid = false, console.log("6");
                if (this.errorMessages.items != null && this.order.orderItems === null)
                     valid =   false, console.log("7");

                return valid;

            },
        }
    };
</script>