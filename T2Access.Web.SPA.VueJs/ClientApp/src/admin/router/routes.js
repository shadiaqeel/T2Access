import NotFound from '../components/views/NotFound';

const routePrefix = 'en/admin'

const routes = [{
        name: 'home',
        path: `/${routePrefix}/`,
        component: () =>
            import ('../components/views/Home'),
        display: 'Home'
    },
    {
        name: 'Account',
        path: `/${routePrefix}/Account`,
        component: () =>
            import ('../components/views/Account'),
        display: 'Counter'
    },
    { name: '404', path: '/404', component: NotFound },
    { name: 'catchAll', path: '*', redirect: '/404' }
];

export default routes;