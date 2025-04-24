// Функция для скачивания файлов
window.downloadFileFromStream = function (fileName, dataUrl) {
    const link = document.createElement('a');
    link.href = dataUrl;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}; 