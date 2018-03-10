<template>
    <div class="sidebar" data-color="purple" data-image="img/sidebar-1.jpg">
        <div class="logo">
            <a href="http://www.creative-tim.com" class="simple-text">
                AEF Curitiba
            </a>
        </div>
        <div class="sidebar-wrapper">
            <ul class="nav">
                <li v-for="route in menuRoutes" :id="route.idSetActive" class="removeActive">
                    <router-link :to="route.path" v-on:click.native="isActive(route.idSetActive)">
                        <i class="material-icons">dashboard</i>
                        <p>{{ route.display }}</p>
                    </router-link>
                </li>
            </ul>
        </div>
    </div>
</template>
<script>
    import { menuRoutes } from '../menu-routes'

    export default {
        data() {
            return {
                menuRoutes,
                collapsed: true
            }
        },
        methods: {
            toggleCollapsed: function (event) {
                this.collapsed = !this.collapsed;
            },
            isActive: function (id) {
                this.removeActive();

                var el = document.getElementById(id);
                el.classList.add('active');
            },
            removeActive: function () {
                var li = document.getElementsByClassName('removeActive');
                if (li) {
                    for (var i = 0; i < li.length; i++) {
                        li[i].classList.remove('active');
                    }
                }
            }
        },
        mounted() {
            this.removeActive();
        }
    }
</script>
<style>
</style>
