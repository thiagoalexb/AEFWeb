import HomePage from 'components/home-page'
import User from 'components/user'

export const menuRoutes = [
    { path: '/', component: HomePage, display: 'Home', style: 'nc-bank', idSetActive: 'home' },
    { path: '/user', component: User, display: 'User', style: 'nc-circle-09', idSetActive: 'user' },
]
