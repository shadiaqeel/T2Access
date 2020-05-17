import NotFound from '../components/views/NotFound';
import ContentPanel from '../components/layouts/ContentPanel';

const routePrefix = 'admin';

const routes = [{
        path: `${routePrefix}/`,
        component: ContentPanel,
        display: 'Home',
        children: [{
            name: 'home',
            path: '',
            component: () =>
                import ('../components/views/Home')
        }]
    },
    {
        path: `${routePrefix}/user`,
        component: ContentPanel,
        display: 'UserManagement',
        children: [{
                name: 'user',
                path: '',
                component: () =>
                    import ('../components/views/managements/user/UserManagement'),
                children: [{
                    name: 'ResetPasswordUser',
                    path: 'resetPassword/:user',
                    props: true,
                    component: () =>
                        import ('../components/views/managements/user/ResetPassword')
                }]

            },
            {
                name: 'createUser',
                path: 'create',
                component: () =>
                    import ('../components/views/managements/user/Create')
            },
            {
                name: 'EditUser',
                path: 'edit/:userId',
                component: () =>
                    import ('../components/views/managements/user/Edit'),
                props: true
            },

        ]
    },
    {
        path: `${routePrefix}/gate`,
        component: ContentPanel,
        display: 'GateManagement',
        children: [{
            name: 'gate',
            path: '',
            component: () =>
                import ('../components/views/managements/gate/GateManagement'),
            children: [{
                    name: 'createGate',
                    path: 'create',
                    component: () =>
                        import ('../components/views/managements/gate/Create')
                },
                {
                    name: 'EditGate',
                    path: 'edit/:gateId',
                    props: true,
                    component: () =>
                        import ('../components/views/managements/gate/Edit')
                },
                {
                    name: 'ResetPasswordGate',
                    path: 'resetPassword/:gate',
                    props: true,
                    component: () =>
                        import ('../components/views/managements/gate/ResetPassword')
                }
            ]
        }]
    },
    { name: 'logout', path: '/account/logout' },
    { name: '404', path: '/404', component: NotFound },
    { name: 'catchAll', path: '*', redirect: '/404' }
];

export default routes;