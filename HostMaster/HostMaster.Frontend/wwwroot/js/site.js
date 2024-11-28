function downloadFile(fileName, contentType, content) {
    const blob = new Blob([new Uint8Array(content)], { type: contentType });
    const link = document.createElement("a");
    link.href = URL.createObjectURL(blob);
    link.download = fileName;
    link.click();
    URL.revokeObjectURL(link.href);
}
