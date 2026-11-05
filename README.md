# Elvis Photo Uploader

A C# project exploring a hotkey-driven helper for the ELVIS photo-upload interface running in Microsoft Edge IE mode.

## Project Goals

- Detect the ELVIS photo-upload modal
- Identify the browser-rendering component
- Investigate access to existing HTML file inputs
- Eventually select up to 8 photos per batch
- Resize photos to 800×600
- Keep final upload confirmation manual

## Current Progress

- [x] Detect Microsoft Edge process
- [x] Locate photo-upload modal using Win32 APIs
- [x] Identify Internet Explorer_Server child window
- [x] Retrieve UI Automation element
- [x] Investigate UI Automation child elements
- [ ] Investigate MSAA accessibility
- [ ] Establish access to HTML file inputs
- [ ] Implement photo batching and resizing
- [ ] Implement system tray and hotkeys

## Current Limitations

UI Automation successfully retrieves the browser
pane, but child-element searches return no results.

Further investigation is required to determine
whether the HTML document can be accessed.

## Technologies

- C# / .NET
- Windows API (P/Invoke)
- Microsoft UI Automation
- Microsoft Edge IE mode

## Development Status

Early proof of concept. Not ready for production.
Experiments are performed against a local demo.
