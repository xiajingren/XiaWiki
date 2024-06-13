// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// 监听页面滚动事件
window.addEventListener('scroll', function () {
    // 获取视窗的高度
    var viewportHeight = window.innerHeight || document.documentElement.clientHeight;
    // 获取文档body的总高度
    var documentBodyHeight = document.body.offsetHeight;
    // 获取滚动条的垂直位置
    var scrollTopPosition = window.scrollY || document.documentElement.scrollTop || document.body.scrollTop;

    // 输出滚动位置
    console.log('滚动位置:', scrollTopPosition);

    var footerHeight = document.getElementsByTagName("footer")[0].clientHeight;

    // 判断是否到达底部
    if (viewportHeight + scrollTopPosition + footerHeight >= documentBodyHeight) {
        console.log('到达或者超过了底部');
        // 在这里执行到达底部后的操作
        document.getElementById("my-sidebar-box").style.height = `calc(100vh - 6rem - ${viewportHeight + scrollTopPosition + footerHeight - documentBodyHeight - 56}px)`
        document.getElementById("my-outline-box").style.height = `calc(100vh - 6rem - ${viewportHeight + scrollTopPosition + footerHeight - documentBodyHeight - 56}px)`
    } else {
        document.getElementById("my-sidebar-box").style.removeProperty('height')
        document.getElementById("my-outline-box").style.removeProperty('height')
    }
});
