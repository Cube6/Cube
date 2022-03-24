<template>
    <div id="app">
        <router-view></router-view>
        <vue-confirm-dialog style="word-break:break-word"></vue-confirm-dialog>
    </div>
</template>

<script>
    
export default {
}
</script>

<style>
    body {
        background-image: url(./assets/login_bg.jpg);
        background-size: cover;
        height: calc(100% - 10px);
        font-family: -apple-system,BlinkMacSystemFont,Segoe UI,Roboto,Oxygen,Ubuntu,Fira Sans,Droid Sans,Helvetica Neue,sans-serif;;
        margin: 0;
        padding: 0;
    }

</style>
