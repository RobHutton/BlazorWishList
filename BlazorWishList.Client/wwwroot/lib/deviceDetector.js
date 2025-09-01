//window.deviceHelper = {
//    getDeviceInfo: function () {
//        const isMobileDevice = /Android|iPhone|iPad|iPod|Windows Phone/i.test(navigator.userAgent);
//        const isMobileLayout = window.innerWidth <= 768;
//        alert(window.innerWidth);
//        return {
//            windowWidth: window.innerWidth,
//            isMobileDevice: isMobileDevice,
//            isMobileLayout: isMobileLayout,
//            isMobile: isMobileDevice || isMobileLayout
//        };
//    }
//};
window.getDimensions = function () {
    alert("hello");
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
};