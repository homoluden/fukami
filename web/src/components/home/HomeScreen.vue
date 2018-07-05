<template>
  <div class="page-container md-layout-column">
    <md-toolbar class="md-primary">
      <md-button class="md-icon-button" @click="showNavigation = true">
        <md-icon>menu</md-icon>
      </md-button>
      <span class="md-title">Fukami Game Resources</span>

      <div class="md-toolbar-section-end">
        <md-button @click="showSidepanel = true">Favorites</md-button>
      </div>
    </md-toolbar>

    <md-drawer :md-active.sync="showNavigation">
      <md-toolbar class="md-transparent" md-elevation="0">
        <span class="md-title">My App name</span>
      </md-toolbar>

      <md-list>
        <md-list-item>
          <md-icon>move_to_inbox</md-icon>
          <span class="md-list-item-text">Inbox</span>
        </md-list-item>

        <md-list-item>
          <md-icon>send</md-icon>
          <span class="md-list-item-text">Sent Mail</span>
        </md-list-item>

        <md-list-item>
          <md-icon>delete</md-icon>
          <span class="md-list-item-text">Trash</span>
        </md-list-item>

        <md-list-item>
          <md-icon>error</md-icon>
          <span class="md-list-item-text">Spam</span>
        </md-list-item>
      </md-list>
    </md-drawer>

    <md-drawer class="md-right" :md-active.sync="showSidepanel">
      <md-toolbar class="md-transparent" md-elevation="0">
        <span class="md-title">Favorites</span>
      </md-toolbar>

      <md-list>
        <md-list-item>
          <span class="md-list-item-text">Abbey Christansen</span>

          <md-button class="md-icon-button md-list-action">
            <md-icon class="md-primary">chat_bubble</md-icon>
          </md-button>
        </md-list-item>

        <md-list-item>
          <span class="md-list-item-text">Alex Nelson</span>

          <md-button class="md-icon-button md-list-action">
            <md-icon class="md-primary">chat_bubble</md-icon>
          </md-button>
        </md-list-item>

        <md-list-item>
          <span class="md-list-item-text">Mary Johnson</span>

          <md-button class="md-icon-button md-list-action">
            <md-icon>chat_bubble</md-icon>
          </md-button>
        </md-list-item>
      </md-list>
    </md-drawer>

    <md-content class="main-content">
      <md-bottom-bar class="md-alignment-top-center"
        md-type="shift" :md-theme="'bottom-bar-' + theme">
        <md-bottom-bar-item md-label="Ready" md-icon="done_all" @click="theme = 'teal'"></md-bottom-bar-item>
        <md-bottom-bar-item md-label="Drafts" md-icon="rounded_corner" @click="theme = 'orange'"></md-bottom-bar-item>
        <md-bottom-bar-item md-label="Raw" md-icon="block" @click="theme = 'red'"></md-bottom-bar-item>
        <md-bottom-bar-item md-label="Demo" md-icon="developer_mode" @click="theme = 'black'"></md-bottom-bar-item>
      </md-bottom-bar>
    </md-content>
  </div>
</template>

<script>
export default {
  name: 'HomeScreen',
  data() {
    return {
      theme: 'teal',
      showNavigation: false,
      showSidepanel: false,
    };
  },
};
</script>

<style lang="scss">
@import "~vue-material/dist/theme/engine";

$active-border-color: md-get-palette-color(black, 300);
.page-container {
  min-height: 100vh;
  overflow: hidden;
  position: relative;
  border: 1px solid rgba(#000, .12);
}

  // Demo purposes only
.md-drawer {
  width: 230px;
  max-width: calc(100vw - 125px);
}

.md-content {
  padding: 16px;
}

.main-content {
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  padding: 0;
  display: inline-flex;
  align-items: flex-end;
  overflow: hidden;
  border: 1px solid rgba(#000, .26);
  background: rgba(#000, .06);

  .md-bottom-bar .md-bottom-bar-item.md-active {
    width: initial;
    border-top: 5px $active-border-color solid;
  }

  .md-bottom-bar>.md-ripple {
    justify-content: center;
  }
}

</style>

<style lang="scss">
  @import "~vue-material/dist/theme/engine";

  @include md-register-theme("bottom-bar-teal", (
    primary: md-get-palette-color(teal, A200)
  ));

  @include md-register-theme("bottom-bar-orange", (
    primary: md-get-palette-color(orange, A200)
  ));

  @include md-register-theme("bottom-bar-blue", (
    primary: md-get-palette-color(blue, A200),
    accent: md-get-palette-color(red, A200)
  ));

  @include md-register-theme("bottom-bar-red", (
    primary: md-get-palette-color(red, A200)
  ));

  @include md-register-theme("bottom-bar-black", (
    primary: md-get-palette-color(lightblue, 900)
  ));

  @import "~vue-material/dist/theme/all";
</style>
