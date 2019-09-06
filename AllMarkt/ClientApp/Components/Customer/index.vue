<template>
    <topBar ref="topBar" >
        <div class="row">
            <div class="col-1 leftmenu" style="background:#c7daff; height:1000px">
                <div class="nav flex-column nav-pills nav-justified" id="v-pills-tab" role="tablist">
                    <a v-on:click="() => $refs.main.loadShopsAsync(0)"
                       class="nav-link text-dark" id="v-pills-home-tab" data-toggle="pill" href="#" role="tab" aria-selected="false">All Category</a>
                    <a v-for="category in categories" v-bind:key="category.id" v-on:click="() => $refs.main.loadShopsAsync(category.id)"
                       class="nav-link text-dark" id="v-pills-home-tab" data-toggle="pill" href="#" role="tab" aria-selected="false">{{category.name}}</a>
                </div>
            </div>
            <div class="col-11 rightmenu">
                <Main ref="main" v-on:addToCartIndex="addToCart"/>
            </div>
        </div>
    </topBar>
</template>
<script>
    import Api from "../../Api";
    import Main from "../Customer/main.vue";
    import TopBar from "../Header/topBar";
     import UserStore from "../../UserStore";

    export default {
        components: {
            Main,
            TopBar
        },
        data() {
            return {
                categories: []
            };
        },
        methods: {
            async loadAsync() {
                try {
                    const result = await Api.categories.getAllAsync();
                    this.categories = result.data;
                } finally {
                }
            },
            addToCart(item) {
                this.$refs.topBar.addItemToCart(item);
            }
        },
        mounted() {
            this.loadAsync();
            var currentUser = JSON.parse(localStorage.getItem("currentUser"));
            UserStore.userRole = currentUser.userRole;
        }
    }
</script>

