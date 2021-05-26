<template>
    <div>
        <MobileLogin v-if="this.$store.state.mobileStatus === 'login'"/>
        <MobileChat v-if="this.$store.state.mobileStatus === 'chat'"/>
        <MobileRegister v-if="this.$store.state.mobileStatus === 'register'"/>
    </div>
</template>

<script>
import MobileLogin from "./MobileLogin";
import MobileChat from "./MobileChat";
import MobileRegister from "./MobileRegister";

export default {
    name: "Mobile",
    data() {
        return {
            status: 'login',
        }
    },
    components: {
        MobileLogin,
        MobileChat,
        MobileRegister,
    },
    beforeCreate() {
    }
}
</script>

<style scoped>

</style>
