import { INavData } from '@coreui/angular';

export const navItems: INavData[] = [
  {
    name: 'Dashboard',
    url: '/dashboard',
    icon: 'icon-speedometer'
  },
  // {
  //   title: true,
  //   name: 'Theme'
  // },

  // {
  //   name: 'Add Fund',
  //   url: '/base/addfund',
  //   icon: 'cui-dollar'
  // },
  // {
  //   name: 'Activate Phase',
  //   url: '/base/buymatrixposition',
  //   icon: 'icon-basket-loaded'
  // },
  {
    name: 'Buy Package',
    url: '/base/buyadpack',
    icon: 'icon-basket-loaded'
  },
  // {
  //   name: 'My Traffic Pack',
  //   url: '/base/myshares',
  //   icon: 'fa fa-line-chart'
  // },
  // {
  //   name: 'Earn GCT',
  //   url: '/base/myshares',
  //   icon: 'fa fa-dollar'
  // },
  {
    name: 'My Team',
    url: '/base/tree',
    icon: 'fa fa-line-chart'
  },
  {
    name: 'My Direct',
    url: '/base/myteam',
    icon: 'fa fa-users'
  },
   {
     name: 'Withdrawal',
     url: '/base/withdrawal',
    icon: 'fa fa-google-wallet'
   },
   {
     name: 'Transfer Fund',
     url: '/base/transferfund',
    icon: 'fa fa-google-wallet'
   },
   {
    name: 'Transaction Report',
    url: '/base/transferhistory',
   icon: 'fa fa-google-wallet'
  },
   {
     name: 'Booster Coin',
     url: '/base/magiccoin',
    icon: 'fa fa-google-wallet'
   },
  // {
  //   name: 'Change Passowrd',
  //   url: '/base/changepassword',
  //   icon: 'fa fa-lock '
  // },
  // {
  //   name: 'Add Campaign',
  //   url: '/base',
  //   icon: 'cui-cog',
  //   children: [
  //     {
  //       name: 'Login Ads',
  //       url: '/base/loginad',
  //       icon: 'fa fa-bullhorn'
  //     },
  //     {
  //       name: 'Banner 125x125',
  //       url: '/base/banner125x125',
  //       icon: 'fa fa-bullhorn'
  //     },
  //     {
  //       name: 'Banner 728x90',
  //       url: '/base/banner728x90',
  //       icon: 'fa fa-bullhorn'
  //     },
  //     {
  //       name: 'Banner 468x60',
  //       url: '/base/banner468x60',
  //       icon: 'fa fa-bullhorn'
  //     }
  //   ]
  // },
  {
    name: 'Marketing Tools',
    url: '/base/promotion',
    icon: 'cui-globe'
  },
  // {
  //   name: 'Advertisment',
  //   url: '/base/marketing',
  //   icon: 'cui-globe'
  // },
  // {
  //   name: 'Deposit History',
  //   url: '/base/deposithistory',
  //   icon: 'cui-pie-chart'
  // },
  // {
  //   name: 'Withdrawal History',
  //   url: '/base/withdrawalhistory',
  //   icon: 'cui-pie-chart'
  // },
  // {
  //   name: 'Purchase History',
  //   url: '/base/purchasehistory',
  //   icon: 'cui-pie-chart'
  // },
  {
    name:'Account Settings',
    url:'/base/account',
    icon:'icon-settings'
  },
  {
    name:'Security Settings',
    url:'/base/changepassword',
    icon:'icon-settings'
  },
  {
    name: 'Support',
    url: '/base/support',
    icon: 'fa fa-support '
  }
  //,
  // {
  //   name: 'Buttons',
  //   url: '/buttons',
  //   icon: 'icon-cursor',
  //   children: [
  //     {
  //       name: 'Buttons',
  //       url: '/buttons/buttons',
  //       icon: 'icon-cursor'
  //     },
  //     {
  //       name: 'Dropdowns',
  //       url: '/buttons/dropdowns',
  //       icon: 'icon-cursor'
  //     },
  //     {
  //       name: 'Brand Buttons',
  //       url: '/buttons/brand-buttons',
  //       icon: 'icon-cursor'
  //     }
  //   ]
  // },
  // {
  //   name: 'Charts',
  //   url: '/charts',
  //   icon: 'icon-pie-chart'
  // },
  // {
  //   name: 'Icons',
  //   url: '/icons',
  //   icon: 'icon-star',
  //   children: [
  //     {
  //       name: 'CoreUI Icons',
  //       url: '/icons/coreui-icons',
  //       icon: 'icon-star',
  //       badge: {
  //         variant: 'success',
  //         text: 'NEW'
  //       }
  //     },
  //     {
  //       name: 'Flags',
  //       url: '/icons/flags',
  //       icon: 'icon-star'
  //     },
  //     {
  //       name: 'Font Awesome',
  //       url: '/icons/font-awesome',
  //       icon: 'icon-star',
  //       badge: {
  //         variant: 'secondary',
  //         text: '4.7'
  //       }
  //     },
  //     {
  //       name: 'Simple Line Icons',
  //       url: '/icons/simple-line-icons',
  //       icon: 'icon-star'
  //     }
  //   ]
  // },
  // {
  //   name: 'Notifications',
  //   url: '/notifications',
  //   icon: 'icon-bell',
  //   children: [
  //     {
  //       name: 'Alerts',
  //       url: '/notifications/alerts',
  //       icon: 'icon-bell'
  //     },
  //     {
  //       name: 'Badges',
  //       url: '/notifications/badges',
  //       icon: 'icon-bell'
  //     },
  //     {
  //       name: 'Modals',
  //       url: '/notifications/modals',
  //       icon: 'icon-bell'
  //     }
  //   ]
  // },
  // {
  //   name: 'Widgets',
  //   url: '/widgets',
  //   icon: 'icon-calculator',
  //   badge: {
  //     variant: 'info',
  //     text: 'NEW'
  //   }
  // },
  // {
  //   divider: true
  // },
  // {
  //   title: true,
  //   name: 'Extras',
  // },
  // {
  //   name: 'Pages',
  //   url: '/pages',
  //   icon: 'icon-star',
  //   children: [
  //     {
  //       name: 'Login',
  //       url: '/login',
  //       icon: 'icon-star'
  //     },
  //     {
  //       name: 'Register',
  //       url: '/register',
  //       icon: 'icon-star'
  //     },
  //     {
  //       name: 'Error 404',
  //       url: '/404',
  //       icon: 'icon-star'
  //     },
  //     {
  //       name: 'Error 500',
  //       url: '/500',
  //       icon: 'icon-star'
  //     }
  //   ]
  // },
  // {
  //   name: 'Disabled',
  //   url: '/dashboard',
  //   icon: 'icon-ban',
  //   badge: {
  //     variant: 'secondary',
  //     text: 'NEW'
  //   },
  //   attributes: { disabled: true },
  // },
  // {
  //   name: 'Download CoreUI',
  //   url: 'http://coreui.io/angular/',
  //   icon: 'icon-cloud-download',
  //   class: 'mt-auto',
  //   variant: 'success',
  //   attributes: { target: '_blank', rel: 'noopener' }
  // },
  // {
  //   name: 'Try CoreUI PRO',
  //   url: 'http://coreui.io/pro/angular/',
  //   icon: 'icon-layers',
  //   variant: 'danger',
  //   attributes: { target: '_blank', rel: 'noopener' }
  // }
];
