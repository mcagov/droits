/* Import TinyMCE */
var tinymce = require('tinymce');

/* Default icons are required for TinyMCE 5.3 or above. Also import custom icons if applicable */
require('tinymce/icons/default');

/* A editor theme (required) - customize the editor appearance by creating a 'skin' */
require('tinymce/themes/silver');

/* Import the editor skin - replace with a custom skin if applicable. */
require('tinymce/skins/ui/oxide/skin.css');

/* Import plugins - include the relevant plugin in the 'plugins' option. */
require('tinymce/plugins/advlist');
require('tinymce/plugins/code');
require('tinymce/plugins/emoticons');
require('tinymce/plugins/emoticons/js/emojis');
require('tinymce/plugins/link');
require('tinymce/plugins/lists');
require('tinymce/plugins/table');
require('tinymce/plugins/image');
require('tinymce/plugins/anchor');
require('tinymce/models/dom');



// /* Import content CSS */
// var contentUiCss = require('tinymce/skins/ui/oxide/content.css');
//
// /* Import the default content CSS, replace with the CSS for the editor content. */
// var contentCss = require('tinymce/skins/content/default/content.css');
export function initializeTinyMce()
{
    
    // Initialize TinyMCE without jQuery
    tinymce.init({
        selector: '.wysiwyg-editor',
        plugins: 'lists image anchor',
        toolbar: 'undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
        height: 300,
        branding: false,
        promotion: false,
        skin: false,
        content_css: false,
        link_assume_external_targets: true
    });
    tinymce.init({
        selector: '.wysiwyg-editor-readonly',
        plugins: '',
        menubar: false,
        toolbar:false,
        height: 300,
        branding: false,
        promotion: false,
        readonly: true,
        skin: false,
        content_css: false,
        link_assume_external_targets: true

    });
}
