<template>
    <topBar ref="topBar">
        <ul class="nav nav-pills nav-fill m-3">
            <li class="nav-item m-3">
                <router-link to="/categories" class="nav-link" active-class="active">Manage Categories</router-link>
            </li>
            <li class="nav-item m-3">
                <router-link to="/productCategories/shop" class="nav-link" active-class="active">Manage Product Categories</router-link>
            </li>
            <li class="nav-item m-3">
                <router-link to="/products" class="nav-link" active-class="active">Manage Products</router-link>
            </li>
            <li class="nav-item m-3">
                <router-link to="/privateMessages/user" class="nav-link" active-class="active">Private Messages</router-link>
            </li>
        </ul>
        <div>
            <slot/>
        </div>

    </topBar>
</template>

<script>
    import TopBar from "../Header/topBar";
    import UserStore from "../../UserStore";

    export default {
        components: {
            TopBar
        },
        mounted() {
              var currentUser = JSON.parse(localStorage.getItem("currentUser"));
            UserStore.userRole = currentUser.userRole;
        }
    }


</script>