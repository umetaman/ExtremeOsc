import {themes as prismThemes} from 'prism-react-renderer';
import type {Config} from '@docusaurus/types';
import type * as Preset from '@docusaurus/preset-classic';

// This runs in Node.js - Don't use client-side code here (browser APIs, JSX...)

const config: Config = {
  title: '⚡️ ExtremeOsc',
  tagline: 'C# implemetation of OSC (Open Sound Control) for Unity.',

  // Set the production url of your site here
  url: 'https://umetaman.github.io',
  // Set the /<baseUrl>/ pathname under which your site is served
  // For GitHub pages deployment, it is often '/<projectName>/'
  baseUrl: '/ExtremeOsc/',
  organizationName: 'umetaman',
  projectName: 'ExtremeOsc',

  onBrokenLinks: 'throw',
  onBrokenMarkdownLinks: 'warn',

  // Even if you don't use internationalization, you can use this field to set
  // useful metadata like html lang. For example, if your site is Chinese, you
  // may want to replace "en" with "zh-Hans".
  i18n: {
    defaultLocale: 'en',
    locales: ['en', "ja"],
    localeConfigs: {
      en: {
        label: 'English',
        htmlLang: 'en',
      },
      ja: {
        label: '日本語',
        htmlLang: 'ja',
      },
    }
  },

  presets: [
    [
      'classic',
      {
        docs: {
          sidebarPath: './sidebars.ts',
          editUrl:
            'https://github.com/umetaman/ExtremeOsc',
        },
        theme: {
          customCss: './src/css/custom.css',
        },
      } satisfies Preset.Options,
    ],
  ],

  themeConfig: {
    image: 'img/docusaurus-social-card.jpg',
    navbar: {
      title: '⚡️ ExtremeOsc',
      items: [
        {
          type: 'docSidebar',
          sidebarId: 'tutorialSidebar',
          position: 'left',
          label: 'Document',
        },
        {
          type: "localeDropdown",
          position: "right"
        },
        {
          href: 'https://github.com/umetaman/ExtremeOsc',
          label: 'GitHub',
          position: 'right',
        },
      ],
    },
    footer: {
      style: 'dark',
      copyright: `Copyright © ${new Date().getFullYear()} umetaman. Built with Docusaurus.`,
    },
    prism: {
      additionalLanguages: ["csharp"],
      theme: prismThemes.github,
      darkTheme: prismThemes.dracula,
    },
  } satisfies Preset.ThemeConfig,
};

export default config;
