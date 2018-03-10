<template>
    <div id="app" class="height-100">

        <nav-menu params="route: route"></nav-menu>

        <div class="main-panel height-100-scroll">
            <nav class="navbar navbar-red navbar-absolute">
                <div class="container-fluid">
                    <div class="navbar-header">
                        <button type="button" class="navbar-toggle" data-toggle="collapse">
                            <span class="sr-only">Toggle navigation</span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                        </button>
                        <a class="navbar-brand" href="#">AEF</a>
                    </div>
                    <div class="collapse navbar-collapse">
                        <ul class="nav navbar-nav navbar-right">
                            <li class="dropdown">
                                <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                                    <i class="material-icons">person</i>
                                    <p class="hidden-lg hidden-md">Profile</p>
                                </a>
                                <ul class="dropdown-menu">
                                    <li>
                                        <a href="#">Minha conta</a>
                                    </li>
                                    <li>
                                        <a href="#">Sair</a>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>

            <div class="content">
                <div class="container-fluid">
                    <div class="row">
                        <router-view></router-view>
                    </div>
                </div>
            </div>

            <footer class="footer">
                <div class="container-fluid">
                    <p class="copyright pull-right">
                        <a href="#"> © Quintal Labs</a>
                    </p>
                </div>
            </footer>
        </div>
    </div>

</template>
<script>
    import Vue from 'vue'
    import NavMenu from './nav-menu'
    
    Vue.component('nav-menu', NavMenu);

    export default {
        data() {
            return {
            }
        }
    }
</script>
<style>
</style>
