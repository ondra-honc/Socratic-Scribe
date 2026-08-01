const inputField = document.getElementById("inputBox");
const charCounter = document.querySelector(".charCounter");
const analyzeButton = document.getElementById("analyze");
const outputBox = document.getElementById("output");
const modeSelector = document.getElementById("modeSelector");

function executeShake(execute) {
  if (execute) {
    charCounter.classList.add("at-limit");
    inputField.classList.remove("shake");
    void inputField.offsetWidth;
    inputField.classList.add("shake");
  } else {
    inputField.classList.remove("at-limit", "shake");
    charCounter.classList.remove("at-limit");
  }
}

inputField.addEventListener('keydown', (e) => {
  const currentLength = inputField.value.length;
  const maxLength = inputField.getAttribute("maxlength");
  const isControlKey = e.key.length > 1 || e.ctrlKey || e.metaKey || e.altKey;

  executeShake(currentLength >= maxLength && !isControlKey);
});

inputField.addEventListener('input', () => {
  const currentLength = inputField.value.length;
  const maxLength = inputField.getAttribute("maxlength");

  charCounter.textContent = `${currentLength} / ${maxLength}`;
  executeShake(currentLength >= maxLength);
});

analyzeButton.addEventListener('click', async () => {
  const text = inputField.value.trim();
  const deepMode = modeSelector.checked;

  outputBox.value = '';

  if (text.length < 25) {
    executeShake(true);
    setTimeout(() => {
      if (inputField.value.length < inputField.getAttribute("maxlength")) {
        executeShake(false);
      }
    }, 400);

    outputBox.classList.add("error-mode");
    outputBox.value = "Please enter at least 25 characters to analyze your text.";
    return;
  }

  outputBox.value = 'Thinking...';
  analyzeButton.disabled = true;
  let isFirstChunk = true;

  const requestOptions = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ inputText: text, isDeepMode: deepMode })
  };

  try {
    const response = await fetch('/api/analysis', requestOptions);

    if (!response.ok) {
      outputBox.classList.add('error-mode');

      if (response.status === 429) {
        outputBox.value = "You're analyzing text a bit too quickly. Please wait a few seconds and try again.";
      } else if (response.status >= 500) {
        outputBox.value = "Socratic Scribe is temporarily unavailable. Please try again shortly.";
      } else {
        outputBox.value = "Something went wrong while processing your request. Please try again.";
      }

      return;
    }

    outputBox.classList.remove('error-mode');

    const reader = response.body.getReader();
    const decoder = new TextDecoder();
    let buffer = '';

    while (true) {
      const { done, value } = await reader.read();
      if (done) break;

      buffer += decoder.decode(value, { stream: true });

      let doubleNewlineIndex = buffer.indexOf('\n\n');

      while (doubleNewlineIndex !== -1) {
        const sseFrame = buffer.slice(0, doubleNewlineIndex);
        buffer = buffer.slice(doubleNewlineIndex + 2);

        const lines = sseFrame.split('\n');

        for (const line of lines) {
          if (line.startsWith('data: ')) {
            const dataContent = line.substring(6);

            if (dataContent !== '[DONE]') {
              if (isFirstChunk) {
                outputBox.value = '';
                isFirstChunk = false;
              }

              outputBox.value += dataContent;
            }
          }
        }
        doubleNewlineIndex = buffer.indexOf('\n\n');
      }
    }
  } catch (err) {
    if (err.name === 'AbortError') return;

    outputBox.classList.add('error-mode');
    outputBox.value = "Connection lost. Please check your internet connection and try again.";
  } finally {
    analyzeButton.disabled = false;
  }
});