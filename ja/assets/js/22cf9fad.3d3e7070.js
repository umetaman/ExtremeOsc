"use strict";(self.webpackChunkdocuments=self.webpackChunkdocuments||[]).push([[346],{3450:(e,n,r)=>{r.r(n),r.d(n,{assets:()=>i,contentTitle:()=>t,default:()=>p,frontMatter:()=>l,metadata:()=>s,toc:()=>d});const s=JSON.parse('{"id":"send-receive/receive","title":"OSC\u306e\u53d7\u4fe1\uff08OscServer\uff09","description":"\u30b5\u30dd\u30fc\u30c8\u3057\u3066\u3044\u308b\u53d7\u4fe1\u65b9\u6cd5","source":"@site/i18n/ja/docusaurus-plugin-content-docs/current/send-receive/receive.md","sourceDirName":"send-receive","slug":"/send-receive/receive","permalink":"/ExtremeOsc/ja/docs/send-receive/receive","draft":false,"unlisted":false,"editUrl":"https://github.com/umetaman/ExtremeOsc/docs/send-receive/receive.md","tags":[],"version":"current","sidebarPosition":1,"frontMatter":{"title":"OSC\u306e\u53d7\u4fe1\uff08OscServer\uff09","sidebar_position":1},"sidebar":"tutorialSidebar","previous":{"title":"OSC\u306e\u9001\u4fe1\uff08OscClient\uff09","permalink":"/ExtremeOsc/ja/docs/send-receive/send"},"next":{"title":"OSC\u306e\u8aad\u307f\u8fbc\u307f","permalink":"/ExtremeOsc/ja/docs/read-write/read"}}');var c=r(4848),a=r(8453);const l={title:"OSC\u306e\u53d7\u4fe1\uff08OscServer\uff09",sidebar_position:1},t=void 0,i={},d=[{value:"\u30b5\u30dd\u30fc\u30c8\u3057\u3066\u3044\u308b\u53d7\u4fe1\u65b9\u6cd5",id:"\u30b5\u30dd\u30fc\u30c8\u3057\u3066\u3044\u308b\u53d7\u4fe1\u65b9\u6cd5",level:2},{value:"\u30b5\u30dd\u30fc\u30c8\u3057\u3066\u3044\u306a\u3044\u53d7\u4fe1\u65b9\u6cd5",id:"\u30b5\u30dd\u30fc\u30c8\u3057\u3066\u3044\u306a\u3044\u53d7\u4fe1\u65b9\u6cd5",level:2}];function o(e){const n={code:"code",h2:"h2",li:"li",p:"p",pre:"pre",strong:"strong",ul:"ul",...(0,a.R)(),...e.components};return(0,c.jsxs)(c.Fragment,{children:[(0,c.jsx)(n.h2,{id:"\u30b5\u30dd\u30fc\u30c8\u3057\u3066\u3044\u308b\u53d7\u4fe1\u65b9\u6cd5",children:"\u30b5\u30dd\u30fc\u30c8\u3057\u3066\u3044\u308b\u53d7\u4fe1\u65b9\u6cd5"}),"\n",(0,c.jsx)(n.p,{children:(0,c.jsxs)(n.strong,{children:["\u2b55 ",(0,c.jsx)(n.code,{children:"[OscPackable]"}),"\u3092\u4f7f\u7528\u3057\u3066\u3044\u308b\u30af\u30e9\u30b9\u3001\u307e\u305f\u306f",(0,c.jsx)(n.code,{children:"ExtremeOsc.IOscPackable"}),"\u3092\u5b9f\u88c5\u3057\u3066\u3044\u308b\u30af\u30e9\u30b9\u3092\u5f15\u6570\u306b\u3068\u308b"]})}),"\n",(0,c.jsx)(n.pre,{children:(0,c.jsx)(n.code,{className:"language-csharp",children:'[OscCallback("/example")]\r\npublic void OnExample(string address, ExampleData data)\r\n{\r\n\r\n}\n'})}),"\n",(0,c.jsx)(n.p,{children:(0,c.jsxs)(n.strong,{children:["\u2b55 ",(0,c.jsx)(n.code,{children:"object[]"}),"\u3092\u5f15\u6570\u306b\u3068\u308b"]})}),"\n",(0,c.jsx)(n.pre,{children:(0,c.jsx)(n.code,{className:"language-csharp",children:'[OscCallback("/example/objects")]\r\npublic void OnExampleObjects(string address, object[] objects)\r\n{\r\n\r\n}\n'})}),"\n",(0,c.jsx)(n.p,{children:(0,c.jsx)(n.strong,{children:"\u2b55 \u5f15\u6570\u306e\u578b\u306e\u9806\u756a\u304c\u53d7\u4fe1\u3057\u305f\u30c7\u30fc\u30bf\u3068\u4e00\u81f4\u3057\u3066\u3044\u308b"})}),"\n",(0,c.jsx)(n.pre,{children:(0,c.jsx)(n.code,{className:"language-csharp",children:'[OscCallback("/example/arguments")]\r\npublic void OnExampleArguments(string address, int value0, float value1, string value2, bool value3)\r\n{\r\n\r\n}\n'})}),"\n",(0,c.jsx)(n.p,{children:(0,c.jsx)(n.strong,{children:"\u2b55 \u5f15\u6570\u306a\u3057"})}),"\n",(0,c.jsx)(n.pre,{children:(0,c.jsx)(n.code,{className:"language-csharp",children:'[OscCallback("/example/noargument")]\r\npublic void OnExampleNoArgument(string address)\r\n{\r\n\r\n}\n'})}),"\n",(0,c.jsx)(n.p,{children:(0,c.jsx)(n.strong,{children:"\u2b55 1\u3064\u306e\u95a2\u6570\u306b\u3064\u304d\u8907\u6570\u306e\u30a2\u30c9\u30ec\u30b9\u3092\u6307\u5b9a\u3059\u308b"})}),"\n",(0,c.jsx)(n.pre,{children:(0,c.jsx)(n.code,{className:"language-csharp",children:'[OscCallback("/example")]\r\n[OscCallback("/example/another")]\r\nprivate void OnExample(string address, ExampleData data)\r\n{\r\n\r\n}\n'})}),"\n",(0,c.jsx)(n.h2,{id:"\u30b5\u30dd\u30fc\u30c8\u3057\u3066\u3044\u306a\u3044\u53d7\u4fe1\u65b9\u6cd5",children:"\u30b5\u30dd\u30fc\u30c8\u3057\u3066\u3044\u306a\u3044\u53d7\u4fe1\u65b9\u6cd5"}),"\n",(0,c.jsx)(n.p,{children:(0,c.jsx)(n.strong,{children:"\u5236\u7d04"})}),"\n",(0,c.jsxs)(n.ul,{children:["\n",(0,c.jsx)(n.li,{children:"\u30a2\u30c9\u30ec\u30b91\u3064\u306b\u304d\u30b3\u30fc\u30eb\u30d0\u30c3\u30af\u95a2\u6570\u306f1\u3064\u306e\u307f"}),"\n",(0,c.jsxs)(n.li,{children:[(0,c.jsx)(n.code,{children:"[OscPackable]"}),"\u30af\u30e9\u30b9\u306f\u5f15\u6570\u306b1\u3064\u307e\u3067"]}),"\n"]}),"\n",(0,c.jsx)(n.p,{children:(0,c.jsx)(n.strong,{children:"\u274e \u30a2\u30c9\u30ec\u30b9\u3092\u8907\u6570\u306e\u95a2\u6570\u306b\u6307\u5b9a\u3059\u308b"})}),"\n",(0,c.jsx)(n.pre,{children:(0,c.jsx)(n.code,{className:"language-csharp",children:'[OscCallback("/example")]\r\nprivate void OnExample(string address)\r\n{\r\n\r\n}\r\n\r\n// \u274e\r\n[OscCallback("/example/another")]\r\nprivate void OnExampleAnother(string address)\r\n{\r\n\r\n}\n'})}),"\n",(0,c.jsx)(n.p,{children:(0,c.jsxs)(n.strong,{children:["\u274e \u8907\u6570\u306e",(0,c.jsx)(n.code,{children:"[OscPackable]"}),"\u30af\u30e9\u30b9\u3092\u5f15\u6570\u306b\u53d6\u308b"]})}),"\n",(0,c.jsx)(n.pre,{children:(0,c.jsx)(n.code,{className:"language-csharp",children:'[OscCallback("/example/")]\r\nprivate void OnExample(string address, ExampleData data, /* \u274e */ExampleData2 data2)\r\n{\r\n\r\n}\n'})})]})}function p(e={}){const{wrapper:n}={...(0,a.R)(),...e.components};return n?(0,c.jsx)(n,{...e,children:(0,c.jsx)(o,{...e})}):o(e)}},8453:(e,n,r)=>{r.d(n,{R:()=>l,x:()=>t});var s=r(6540);const c={},a=s.createContext(c);function l(e){const n=s.useContext(a);return s.useMemo((function(){return"function"==typeof e?e(n):{...n,...e}}),[n,e])}function t(e){let n;return n=e.disableParentContext?"function"==typeof e.components?e.components(c):e.components||c:l(e.components),s.createElement(a.Provider,{value:n},e.children)}}}]);