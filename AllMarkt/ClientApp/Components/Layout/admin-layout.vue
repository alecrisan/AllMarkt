<template>
    <div class="mx-5 my-3">
        <h1 v-if="this.currentUser !== null">Mr {{this.currentUser.displayName}}, we've been expecting you...</h1>
        <!--<h4 ></h4>-->

        <ul class="nav nav-pills">
            <li class="nav-item">
                <router-link to="/" class="nav-link" active-class="active" exact>Home</router-link>
            </li>
            <li class="nav-item">
                <router-link to="/categories" class="nav-link" active-class="active">Categories</router-link>
            </li>
            <li class="nav-item dropdown">
                <a class="nav-link dropdown-toggle" :class="{active: $route.path.includes('/privateMessages/')}" data-toggle="dropdown" href="#" role="button" aria-haspopup="true" aria-expanded="false">PrivateMessages</a>
                <div class="dropdown-menu">
                    <!--<a class="dropdown-item" href="#">
                        <router-link to="/privateMessages/sent">Sent PMs</router-link>
                    </a>
                    <a class="dropdown-item" href="#">
                        <router-link to="/privateMessages/received">Received PMs</router-link>
                    </a>-->
                    <a class="dropdown-item" href="#">
                        <router-link to="/privateMessages/all">All PMs from all users</router-link>
                    </a>
                    <a class="dropdown-item" href="#">
                        <router-link to="/privateMessages/user">User Private Messages</router-link>
                    </a>
                </div>
            </li>

            <li class="nav-item">
                <router-link to="/productCategories" class="nav-link" active-class="active">Product Categories</router-link>
            </li>

            <li class="nav-item">
                <router-link to="/products" class="nav-link" active-class="active">Products</router-link>
            </li>

            <!--<li class="nav-item">
                <router-link to="/shopComments" class="nav-link" active-class="active">ShopComments</router-link>
            </li>-->

            <li class="nav-item">
                <!--<a class="nav-link dropdown-toggle" :class="{active: $route.path.includes('/orders/')}" data-toggle="dropdown" href="#" role="button" aria-haspopup="true" aria-expanded="false">Orders</a>
                <div class="dropdown-menu">
                    <a class="dropdown-item" href="#">
                        <router-link to="/orders/history">Order history</router-link>
                    </a>
                    <a class="dropdown-item" href="#">

                    </a>
                </div>-->
                <router-link to="/orders" class="nav-link" active-class="active">Orders</router-link>
            </li>

            <li class="nav-item">
                <router-link to="/shops" class="nav-link" active-class="active">Shops</router-link>
            </li>
            <li class="nav-item">
                <router-link to="/productComments" class="nav-link" active-class="active">ProductComments</router-link>
            </li>
            <li class="nav-item">
                <router-link to="/users" class="nav-link" active-class="active">Users</router-link>
            </li>
            <li v-if="!(this.currentUser === null || this.currentUser.token === null)" class="nav-item">
                <button v-on:click="logout" class="nav-link btn " active-class="active">Logout</button>
            </li>
            <!--<li class="nav-item">
                <router-link to="/profileSettings" class="nav-link" active-class="active">Profile Settings</router-link>
            </li>-->
            <li v-if="this.currentUser === null || this.currentUser.token === null" class="nav-item">
                <router-link to="/register" class="nav-link" active-class="active">Register</router-link>
            </li>
        </ul>
        <div class="my-3">
            <slot />
        </div>
    </div>
</template>
<script>
    export default {
        data() {
            return {
                interval: null,
                currentUser: null,
                isLoading: true
            };
        },

        mounted() {
            this.currentUser = JSON.parse(localStorage.getItem("currentUser"));
        },

        methods: {
            logout() {
                var anonymousUser = {
                    token: null,
                    email: null,
                    displayName: null,
                    userRole: null
                };
                localStorage.setItem("currentUser", JSON.stringify(anonymousUser));
                this.$router.go('/');
                this.currentUser = null;
            }
        }
    };
</script>