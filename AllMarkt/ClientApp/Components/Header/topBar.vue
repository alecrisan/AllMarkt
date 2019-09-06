<template>
    <div>
        <div class="d-flex justify-content-between" style="background-color:#1960d1">
            <div>
                <img src="https://i.imgur.com/xdVSouE.png" height="100" />
            </div>

            <div class="d-flex align-items-center">
                <input v-model="searchField" size="100" type="text" placeholder="Search..." aria-label="Small" />
                <button v-on:click="search">
                    <img src="https://cdn3.iconfinder.com/data/icons/shopping-2/256/Searching-512.png" width="20" />
                </button>
            </div>

            <div class="d-flex align-items-center p-2">
                <div v-if="displayCartButton()" class="p-3 w-100">
                    <button class="btn btn-info" v-on:click="openCart">
                        Shopping Cart
                        <span v-if="totalNrOfProducts>0" class="badge badge-warning">{{this.totalNrOfProducts}}</span>
                    </button>
                </div>
                <div v-if="user.userRole != null" class="dropdown">
                    <button class="btn btn-info dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        Hello, {{this.user.name}}
                    </button>
                    <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                        <a class="dropdown-item" href="#" v-if="user.userRole === 'Customer'" v-on:click ="goTo('/customer')">
                           Home
                        </a>
                        <a class="dropdown-item" href="#">
                            <router-link to="/profileSettings" style="text-decoration:none; color:black">Profile Settings</router-link>
                        </a>
                        <a class="dropdown-item" href="#">
                            <router-link to="/orders/history" style="text-decoration:none; color:black">Order History</router-link>
                        </a>
                        <a class="dropdown-item" href="#" v-if="user.userRole === 'Customer'">
                            <router-link to="/privateMessages/user" style="text-decoration:none; color:black">Private Messages</router-link>
                        </a>
                        <a class="dropdown-item" href="#" v-on:click="logout()">Log Out</a>
                    </div>
                </div>
            </div>
        </div>
        <cart-modal ref="cartModal" v-on:changeNr="changeNr" />
        <div>
            <slot />
        </div>
    </div>
</template>
<script>
    import CartModal from "../ShoppingCart/cart-modal.vue";
     import UserStore from "../../UserStore";
    export default {
        components: {
            CartModal
        },
        data() {
            return {
                searchField: "",
                cart: [],
                totalNrOfProducts: 0,
                user: {
                    name: "",
                    phoneNumber: "placeholder",
                    address: "placeholder",
                    userRole: ""
                },

            }

        },
        mounted() {
            var currentUser = JSON.parse(localStorage.getItem("currentUser"));
            UserStore.userRole = currentUser.userRole;
            this.user.name = currentUser.displayName;
            this.user.userRole = currentUser.userRole;
        },
        methods: {
            goTo(string) {
                this.$router.push({ path: string });
            },
        
            logout() {
                var anonymousUser = {
                    token: null,
                    email: null,
                    displayName: null,
                    userRole: null
                };
                localStorage.setItem("currentUser", JSON.stringify(anonymousUser));
                this.currentUser = null;
                this.$router.push('/');
            },
            displayCartButton() {
                if (UserStore.userRole === "Customer") {
                    return true;
                }
                else {
                    return false;
                }
            },
            addItemToCart(item) {
                this.totalNrOfProducts += item.quantity;
                if (this.cart.length == 0) {
                    var orderItem = { product: item.product, quantity: item.quantity, price: item.product.price * item.quantity };
                    var order = { shop: item.shop, orderItems: [orderItem], totalPrice: orderItem.price, toSend: null };
                    this.cart.push(order);
                } else {
                    var orderItem = { product: item.product, quantity: item.quantity, price: item.product.price * item.quantity };
                    var foundShop = false;
                    for (var i = 0; i < this.cart.length; i++) {
                        if (this.cart[i].shop.id === item.shop.id) {
                            var foundProduct = false;
                            for (var j = 0; j < this.cart[i].orderItems.length; j++) {
                                if (this.cart[i].orderItems[j].product.id === orderItem.product.id) {
                                    this.cart[i].orderItems[j].quantity = parseInt(this.cart[i].orderItems[j].quantity) + parseInt(orderItem.quantity);
                                    this.cart[i].orderItems[j].price += orderItem.price;
                                    this.cart[i].totalPrice += orderItem.price;
                                    foundProduct = true;
                                }
                            }
                            if (!foundProduct) {
                                this.cart[i].orderItems.push(orderItem);
                                this.cart[i].totalPrice += orderItem.price;
                            }
                            foundShop = true;
                        }
                    }
                    if (!foundShop) {
                        var order = { shop: item.shop, orderItems: [orderItem], totalPrice: orderItem.price, toSend:null };
                        this.cart.push(order);
                    }
                }
            },
            openCart() {
                this.$refs.cartModal.show(this.cart, this.user);
            },
            search() {
                alert("The product you search is : " + this.searchField);
            },
            changeNr(quantity) {
                this.totalNrOfProducts = quantity;
            }
        }
    }
</script>