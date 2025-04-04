(function () {

var defs = {}; // id -> {dependencies, definition, instance (possibly undefined)}

// Used when there is no 'main' module.
// The name is probably (hopefully) unique so minification removes for releases.
var register_3795 = function (id) {
  var module = dem(id);
  var fragments = id.split('.');
  var target = Function('return this;')();
  for (var i = 0; i < fragments.length - 1; ++i) {
    if (target[fragments[i]] === undefined)
      target[fragments[i]] = {};
    target = target[fragments[i]];
  }
  target[fragments[fragments.length - 1]] = module;
};

var instantiate = function (id) {
  var actual = defs[id];
  var dependencies = actual.deps;
  var definition = actual.defn;
  var len = dependencies.length;
  var instances = new Array(len);
  for (var i = 0; i < len; ++i)
    instances[i] = dem(dependencies[i]);
  var defResult = definition.apply(null, instances);
  if (defResult === undefined)
     throw 'module [' + id + '] returned undefined';
  actual.instance = defResult;
};

var def = function (id, dependencies, definition) {
  if (typeof id !== 'string')
    throw 'module id must be a string';
  else if (dependencies === undefined)
    throw 'no dependencies for ' + id;
  else if (definition === undefined)
    throw 'no definition function for ' + id;
  defs[id] = {
    deps: dependencies,
    defn: definition,
    instance: undefined
  };
};

var dem = function (id) {
  var actual = defs[id];
  if (actual === undefined)
    throw 'module [' + id + '] was undefined';
  else if (actual.instance === undefined)
    instantiate(id);
  return actual.instance;
};

var req = function (ids, callback) {
  var len = ids.length;
  var instances = new Array(len);
  for (var i = 0; i < len; ++i)
    instances.push(dem(ids[i]));
  callback.apply(null, callback);
};

var ephox = {};

ephox.bolt = {
  module: {
    api: {
      define: def,
      require: req,
      demand: dem
    }
  }
};

var define = def;
var require = req;
var demand = dem;
// this helps with minificiation when using a lot of global references
var defineGlobal = function (id, ref) {
  define(id, [], function () { return ref; });
};
/*jsc
["tinymce.plugins.bbcode.Plugin","tinymce.core.PluginManager","tinymce.core.util.Tools","global!tinymce.util.Tools.resolve"]
jsc*/
defineGlobal("global!tinymce.util.Tools.resolve", tinymce.util.Tools.resolve);


define(
  'tinymce.core.PluginManager',
  [
    'global!tinymce.util.Tools.resolve'
  ],
  function (resolve) {
    return resolve('tinymce.PluginManager');
  }
);



define(
  'tinymce.core.util.Tools',
  [
    'global!tinymce.util.Tools.resolve'
  ],
  function (resolve) {
    return resolve('tinymce.util.Tools');
  }
);



/**
 * This class contains all core logic for the bbcode plugin.
 *
 * @class tinymce.bbcode.Plugin
 * @private
 */
define(
  'tinymce.plugins.bbcode.Plugin',
  [
    'tinymce.core.PluginManager',
    'tinymce.core.util.Tools'
  ],
  function (PluginManager, Tools) {
    PluginManager.add('bbcode', function () {
      return {
        init: function (ed) {
          var self = this, dialect = ed.getParam('bbcode_dialect', 'punbb').toLowerCase();

          ed.on('beforeSetContent', function (e) {
            e.content = self['_' + dialect + '_bbcode2html'](e.content);
          });

          ed.on('postProcess', function (e) {
            if (e.set) {
              e.content = self['_' + dialect + '_bbcode2html'](e.content);
            }

            if (e.get) {
              e.content = self['_' + dialect + '_html2bbcode'](e.content);
            }
          });
        },

        getInfo: function () {
          return {
            longname: 'BBCode Plugin',
            author: 'Ephox Corp',
            authorurl: 'http://www.tinymce.com',
            infourl: 'http://www.tinymce.com/wiki.php/Plugin:bbcode'
          };
        },

        // Private methods

        // HTML -> BBCode in PunBB dialect
        _punbb_html2bbcode: function (s) {
          s = Tools.trim(s);

          function rep(re, str) {
            s = s.replace(re, str);
          }

          // example: <strong> to [b]
          rep(/<a.*?href=\"(.*?)\".*?>(.*?)<\/a>/gi, "[url=$1]$2[/url]");
          rep(/<font.*?color=\"(.*?)\".*?class=\"codeStyle\".*?>(.*?)<\/font>/gi, "[code][color=$1]$2[/color][/code]");
          rep(/<font.*?color=\"(.*?)\".*?class=\"quoteStyle\".*?>(.*?)<\/font>/gi, "[quote][color=$1]$2[/color][/quote]");
          rep(/<font.*?class=\"codeStyle\".*?color=\"(.*?)\".*?>(.*?)<\/font>/gi, "[code][color=$1]$2[/color][/code]");
          rep(/<font.*?class=\"quoteStyle\".*?color=\"(.*?)\".*?>(.*?)<\/font>/gi, "[quote][color=$1]$2[/color][/quote]");
          rep(/<span style=\"color: ?(.*?);\">(.*?)<\/span>/gi, "[color=$1]$2[/color]");
          rep(/<font.*?color=\"(.*?)\".*?>(.*?)<\/font>/gi, "[color=$1]$2[/color]");
          rep(/<span style=\"font-size:(.*?);\">(.*?)<\/span>/gi, "[size=$1]$2[/size]");
          rep(/<font>(.*?)<\/font>/gi, "$1");
          rep(/<img.*?src=\"(.*?)\".*?\/>/gi, "[img]$1[/img]");
          rep(/<span class=\"codeStyle\">(.*?)<\/span>/gi, "[code]$1[/code]");
          rep(/<span class=\"quoteStyle\">(.*?)<\/span>/gi, "[quote]$1[/quote]");
          rep(/<strong class=\"codeStyle\">(.*?)<\/strong>/gi, "[code][b]$1[/b][/code]");
          rep(/<strong class=\"quoteStyle\">(.*?)<\/strong>/gi, "[quote][b]$1[/b][/quote]");
          rep(/<em class=\"codeStyle\">(.*?)<\/em>/gi, "[code][i]$1[/i][/code]");
          rep(/<em class=\"quoteStyle\">(.*?)<\/em>/gi, "[quote][i]$1[/i][/quote]");
          rep(/<u class=\"codeStyle\">(.*?)<\/u>/gi, "[code][u]$1[/u][/code]");
          rep(/<u class=\"quoteStyle\">(.*?)<\/u>/gi, "[quote][u]$1[/u][/quote]");
          rep(/<\/(strong|b)>/gi, "[/b]");
          rep(/<(strong|b)>/gi, "[b]");
          rep(/<\/(em|i)>/gi, "[/i]");
          rep(/<(em|i)>/gi, "[i]");
          rep(/<\/u>/gi, "[/u]");
          rep(/<span style=\"text-decoration: ?underline;\">(.*?)<\/span>/gi, "[u]$1[/u]");
          rep(/<u>/gi, "[u]");
          rep(/<blockquote[^>]*>/gi, "[quote]");
          rep(/<\/blockquote>/gi, "[/quote]");
          rep(/<br \/>/gi, "\n");
          rep(/<br\/>/gi, "\n");
          rep(/<br>/gi, "\n");
          rep(/<p>/gi, "");
          rep(/<\/p>/gi, "\n");
          rep(/&nbsp;|\u00a0/gi, " ");
          rep(/&quot;/gi, "\"");
          rep(/&lt;/gi, "<");
          rep(/&gt;/gi, ">");
          rep(/&amp;/gi, "&");

          return s;
        },

        // BBCode -> HTML from PunBB dialect
        _punbb_bbcode2html: function (s) {
          s = Tools.trim(s);

          function rep(re, str) {
            s = s.replace(re, str);
          }

          // example: [b] to <strong>
          rep(/\n/gi, "<br />");
          rep(/\[b\]/gi, "<strong>");
          rep(/\[\/b\]/gi, "</strong>");
          rep(/\[i\]/gi, "<em>");
          rep(/\[\/i\]/gi, "</em>");
          rep(/\[u\]/gi, "<u>");
          rep(/\[\/u\]/gi, "</u>");
          rep(/\[url=([^\]]+)\](.*?)\[\/url\]/gi, "<a href=\"$1\">$2</a>");
          rep(/\[url\](.*?)\[\/url\]/gi, "<a href=\"$1\">$1</a>");
          rep(/\[img\](.*?)\[\/img\]/gi, "<img src=\"$1\" />");
          rep(/\[color=(.*?)\](.*?)\[\/color\]/gi, "<font color=\"$1\">$2</font>");
          rep(/\[code\](.*?)\[\/code\]/gi, "<span class=\"codeStyle\">$1</span>&nbsp;");
          rep(/\[quote.*?\](.*?)\[\/quote\]/gi, "<span class=\"quoteStyle\">$1</span>&nbsp;");

          return s;
        }
      };
    });
    return function () { };
  }
);
dem('tinymce.plugins.bbcode.Plugin')();
})();
