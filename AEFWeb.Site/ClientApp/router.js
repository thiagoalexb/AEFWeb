import Vue from 'vue'
import VueRouter from 'vue-router'

import { anotherRoutes } from './another-routes'
import { menuRoutes } from './menu-routes'

Vue.use(VueRouter);

let routes = anotherRoutes.concat(menuRoutes);

let router = new VueRouter({
    mode: 'history',
    routes
})

export default router
