<template>
    
        <div v-if="this.currentUser === null || this.currentUser.token === null">
            <login />
        </div>
        <div v-else>
            <layout>
                Hello, {{this.currentUser.displayName}}!
            </layout>
        </div>
   
</template>
<script>
 
    import Layout from "../Layout";
    import Login from "../login";
    import UserStore from "../../UserStore";

    export default {
        components: {
            Layout,
            Login
        },
        data() {
            return {
                currentUser: null,
                interval: null
            }
        },
        mounted() {
            this.currentUser = JSON.parse(localStorage.getItem("currentUser"));
            UserStore.userRole = this.currentUser.userRole;
        },
        beforeUpdate() {
            if (UserStore.userRole === "Shop") {
                this.$router.push("/shopView");
            }
            if (UserStore.userRole === "Customer") {
                this.$router.push("/customer");
            }
        }
    };
</script>