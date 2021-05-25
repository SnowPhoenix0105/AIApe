<template>
    <div>
        <MobileLogin v-if="this.$store.state.mobileStatus === 'login'"/>
        <MobileChat v-if="this.$store.state.mobileStatus === 'chat'"/>
    </div>
</template>

<script>
import MobileLogin from "./MobileLogin";
import MobileChat from "./MobileChat";
// import global from './Global'

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
        // global
    },
    beforeCreate() {
    }
}
</script>

<style scoped>

</style>
