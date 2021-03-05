// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false,
  siteUrl : "http://localhost:4200",
  baseApiUrl: 'http://localhost:53161/api/v1/',
  // siteUrl : "https://magicbooster.net",
  // baseApiUrl: 'https://api.magicbooster.net/api/v1/',
  coinPaymentMerAcc : '7abf1e36724948f8dfb5686a3395cbb0',
  withdrawalFees:0.5,
  AllowFund:true,
  AllowPurchase:true,
  AllowAdPack:true,
  Banner125:'assets/img/125.gif',
  Banner468:'assets/img/468.gif',
  Banner728:'assets/img/728.gif',
  Banner250:'assets/img/250.gif',
  BannerFB:'assets/img/fb.jpg',
  CompanyPresentation:'assets/companypresentation.pdf',
  telegram : "telelink",
  facebook : "facebook",
  twitter : "twitter",
  Managers :[],
  Phases : [
    {
      Name : "Phase 1",
      Cost : 10,
      Levels : [
        {LevelNo:1,Member:2,Commission:0.5},
        {LevelNo:2,Member:4,Commission:0.5},
        {LevelNo:3,Member:8,Commission:0.5},
        {LevelNo:4,Member:16,Commission:0.5},
        {LevelNo:5,Member:32,Commission:3},
      ],
      DirectBonus : 2,
      TotalEarning : 111,
      TableColor : "pink"
    },
    {
      Name : "Phase 2",
      Cost : 20,
      Levels : [
        {LevelNo:1,Member:2,Commission:3},
        {LevelNo:2,Member:4,Commission:3},
        {LevelNo:3,Member:8,Commission:3},
        {LevelNo:4,Member:16,Commission:3},
        {LevelNo:5,Member:32,Commission:3},
        {LevelNo:6,Member:64,Commission:3},
      ],
      DirectBonus : 2,
      TotalEarning : 378,
      TableColor : "blue"
    },
    {
      Name : "Phase 3",
      Cost : 50,
      Levels : [
        {LevelNo:1,Member:2,Commission:6},
        {LevelNo:2,Member:4,Commission:6},
        {LevelNo:3,Member:8,Commission:6},
        {LevelNo:4,Member:16,Commission:6},
        {LevelNo:5,Member:32,Commission:6},
        {LevelNo:6,Member:64,Commission:6},
        {LevelNo:7,Member:128,Commission:6},
      ],
      DirectBonus : 8,
      TotalEarning : 1524,
      TableColor : "pink"
    },
    {
      Name : "Phase 4",
      Cost : 200,
      Levels : [
        {LevelNo:1,Member:2,Commission:20},
        {LevelNo:2,Member:4,Commission:20},
        {LevelNo:3,Member:8,Commission:20},
        {LevelNo:4,Member:16,Commission:20},
        {LevelNo:5,Member:32,Commission:20},
        {LevelNo:6,Member:64,Commission:20},
        {LevelNo:7,Member:128,Commission:20},
        {LevelNo:8,Member:256,Commission:20},
      ],
      DirectBonus : 40,
      TotalEarning : 10200,
      TableColor : "blue"
    },
    {
      Name : "Phase 5",
      Cost : 600,
      Levels : [
        {LevelNo:1,Member:2,Commission:50},
        {LevelNo:2,Member:4,Commission:50},
        {LevelNo:3,Member:8,Commission:50},
        {LevelNo:4,Member:16,Commission:50},
        {LevelNo:5,Member:32,Commission:50},
        {LevelNo:6,Member:64,Commission:50},
        {LevelNo:7,Member:128,Commission:50},
        {LevelNo:8,Member:256,Commission:50},
        {LevelNo:9,Member:512,Commission:50},
      ],
      DirectBonus : 150,
      TotalEarning : 51100,
      TableColor : "pink"
    },
    {
      Name : "Phase 6",
      Cost : 1500,
      Levels : [
        {LevelNo:1,Member:2,Commission:120},
        {LevelNo:2,Member:4,Commission:120},
        {LevelNo:3,Member:8,Commission:120},
        {LevelNo:4,Member:16,Commission:120},
        {LevelNo:5,Member:32,Commission:120},
        {LevelNo:6,Member:64,Commission:120},
        {LevelNo:7,Member:128,Commission:120},
        {LevelNo:8,Member:256,Commission:120},
        {LevelNo:9,Member:512,Commission:120},
        {LevelNo:10,Member:1024,Commission:120},
      ],
      DirectBonus : 300,
      TotalEarning : 243840,
      TableColor : "blue"
    }
  ]
};
