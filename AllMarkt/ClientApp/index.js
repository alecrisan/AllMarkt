import "bootstrap";
import Vue from "vue";
import VueRouter from "vue-router";
import Vuelidate from "vuelidate";

import "./style.scss";
import Home from "./Components/Home";
import Categories from "./Components/Categories";
import SentPrivateMessagesByUser from "./Components/PrivateMessages/sentBy.vue";
import Orders from "./Components/Orders";
import ShopComments from "./Components/ShopComments";
import Products from "./Components/Products";
import ReceivedPrivateMessagesByUser from "./Components/PrivateMessages/receivedBy.vue";
import AllPrivateMessages from "./Components/PrivateMessages/allPrivateMessages.vue";
import Shops from "./Components/Shops/allShops.vue";
import ProductComments from "./Components/ProductComments";
import Users from "./Components/Users";
import AllProductCategories from "./Components/ProductCategories/all.vue";
import ProductCategoriesbyShop from "./Components/ProductCategories/byShop.vue";
import StarRating from "vue-star-rating";
import UserPrivateMessages from "./Components/PrivateMessages/userPrivateMessages.vue";
import Customer from "./Components/Customer";
import ShopPage from "./Components/Customer/shopPage.vue";
import OrderHistory from "./Components/Orders/orderHistory.vue";
import Register from "./Components/Register";
import ShopView from "./Components/ShopView";
import ProfileSettings from "./Components/ProfileSettings";

Vue.component('star-rating', StarRating);

Vue.use(VueRouter);
Vue.use(Vuelidate);

const router = new VueRouter({
    routes: [
        {
            path: '/',
            component: Home
        },
        {
            path: '/categories',
            component: Categories
        },
        {
            path: '/privateMessages/sent',
            component: SentPrivateMessagesByUser
        },
        {
            path: '/privateMessages/received',
            component: ReceivedPrivateMessagesByUser
        },
        {
            path: '/products',
            component: Products
        },
        {
            path: '/privateMessages/all',
            component: AllPrivateMessages
        },
        {
            path: '/shopComments',
            component: ShopComments
        },
        {
            path: '/orders',
            component: Orders
        },
        {
            path: '/orders/history',
            component: OrderHistory
        },
        {
            path: '/shops',
            component: Shops
        },
        {
            path: '/productComments',
            component: ProductComments
        },
        {
            path: '/users',
            component: Users,
            meta: { authorize: ['Admin'] } 
        },
        {
            path: '/productCategories',
            component: AllProductCategories
        },
        {
            path: '/productCategories/shop',
            component: ProductCategoriesbyShop
        },
        {
            path: '/productCategories/shop/:id',
            component: ProductCategoriesbyShop
        },
        {
            path: '/privateMessages/user',
            component: UserPrivateMessages
        },
        {
            path: '/customer',
            component: Customer
        },
        {
            path: '/shopPage',
            component: ShopPage
        },
        {
            path: '/profileSettings',
            component: ProfileSettings
        },
        {
            path: '/register',
            component: Register
        },
        {
            path: '/shopView',
            component: ShopView
        }
    ]
});

const appElement = document.body.appendChild(document.createElement("router-view"));
const vueApp = new Vue({ router });

vueApp.$mount(appElement);