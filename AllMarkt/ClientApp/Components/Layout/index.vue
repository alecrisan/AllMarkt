<template>
    <admin-layout v-if="userRole == 'Admin' || userRole == 'Moderator'">
        <slot />
    </admin-layout>
    <shop-view v-else-if="userRole == 'Shop'">
        <slot />
    </shop-view>
    <top-bar v-else-if="userRole =='Customer'">
        <slot />
    </top-bar>
    <top-bar v-else>
        <slot />
    </top-bar>
</template>
<script>
    import AdminLayout from './admin-layout';
    import ShopView from '../ShopView';
    import TopBar from '../Header/topBar.vue';

    import UserStore from '../../UserStore';

    export default {
        data() {
            return {
                userRole: UserStore.userRole
            };
        },

        components: {
            AdminLayout,
            ShopView,
            TopBar
        },

        created() {
            UserStore.on('userRoleChanged', () => { this.userRole = UserStore.userRole });
        }
    };
</script>