import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import AboutView from '../views/about/AboutView.vue'
import BookingView from '../views/booking/BookingView.vue'
import SportsView from '../views/sports/SportsView.vue'
import ForumView from '../views/forum/ForumView.vue'
import CreateSportEventViewVue from '../views/createsportevent/CreateSportEventView.vue'
import Login from "../components/Login.vue";
import Register from "../components/Register.vue";




const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'home',
    component: HomeView
    },
    {
        path: '/bookings',
        name: 'bookings',
        component: BookingView
    },
    {
        path: '/sports',
        name: 'sports',
        component: SportsView
    },
    {
        path: '/forum',
        name: 'forum',
        component: ForumView
    },
    {
        path: '/about',
        name: 'about',
        component: AboutView
    },
    {
        path: '/createsportevent',
        name: 'createsportevent',
        component: CreateSportEventViewVue
    },
    {
        path: '/login',
        name: 'login',
        component: Login
    },
    {
        path: '/register',
        name: 'register',
        component: Register
    },
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
