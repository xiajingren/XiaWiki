// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// 监听页面滚动事件
window.addEventListener('scroll', function () {
    // 获取视窗的高度
    const viewportHeight = window.innerHeight || document.documentElement.clientHeight;
    // 获取文档body的总高度
    const documentBodyHeight = document.body.offsetHeight;
    // 获取滚动条的垂直位置
    const scrollTopPosition = window.scrollY || document.documentElement.scrollTop || document.body.scrollTop;

    // 输出滚动位置
    // console.log('滚动位置:', scrollTopPosition);

    const footerHeight = document.getElementsByTagName("footer")[0].clientHeight;

    // 判断是否到达底部
    if (viewportHeight + scrollTopPosition + footerHeight >= documentBodyHeight) {
        // console.log('到达或者超过了底部');
        // 在这里执行到达底部后的操作
        document.getElementById("my-sidebar-box").style.height = `calc(100vh - 6rem - ${viewportHeight + scrollTopPosition + footerHeight - documentBodyHeight - 56}px)`
        document.getElementById("my-outline-box").style.height = `calc(100vh - 6rem - ${viewportHeight + scrollTopPosition + footerHeight - documentBodyHeight - 56}px)`
    } else {
        document.getElementById("my-sidebar-box").style.removeProperty('height')
        document.getElementById("my-outline-box").style.removeProperty('height')
    }
});

window.onload = function () {
    const collapseElementList = document.querySelectorAll('.collapse')

    collapseElementList.forEach(collapseEl => {
        if (localStorage.getItem(collapseEl.id) === "show")
            new bootstrap.Collapse(collapseEl, { toggle: true });

        collapseEl.addEventListener('shown.bs.collapse', () => {
            localStorage.setItem(collapseEl.id, "show")
        })
        collapseEl.addEventListener('hidden.bs.collapse', () => {
            localStorage.removeItem(collapseEl.id)
        })
    });

    document.getElementById("my-search-btn").onclick = function () {
        window.location.href = "/s/" + document.getElementById("my-search-input").value;
    }
}
