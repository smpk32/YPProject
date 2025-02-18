/*
 * 욕설을 하트로 변경합니다.
 */
BadWordFilter.Replace = (function(text,language){
	/*let text = params.text;
		let language = params.language;*/
		
		let db = BadWordFilter.DB[language];
		
		if (text !== undefined && db !== undefined) {
			
			let check = (badWord, newWord) => {
				
				for (let i = 0; i < text.length; i += 1) {
					
					let isMatched = true;
					let spaceCount = 0;
					let fcCount = 0;
					
					for (let j = 0; j < badWord.length; j += 1) {
						
						if (i + j + fcCount >= text.length) {
							isMatched = false;
							break;
						}
						
						else {
							
							let c = text[i + j + fcCount];
							
							// 영어가 아닌 경우 띄어쓰기 무시
							if (language !== 'en' && c === ' ') {
								spaceCount += 1;
								fcCount += 1;
								j -= 1;
							}
							
							// 한국어는 숫자 및 특수문자도 막기
							else if (language === 'ko' && '1234567890`~!@#$%^&*()-_=+[{]}\\|	;:\'",<.>/?'.indexOf(c) !== -1) {
								fcCount += 1;
								j -= 1;
							}
							
							else if (c.toLowerCase() !== badWord[j]) {
								isMatched = false;
								break;
							}
						}
					}
					
					if (isMatched === true) {
						
						let start = i;
						let last = i + badWord.length - 1 + fcCount;
						
						// 영어의 경우 좌우가 문자열의 끝이거나 띄어쓰기가 있어야 함
						// 대소문자로 단어 체크
						if (language === 'en') {
							if (
								(start === 0 || text[start - 1] === ' ' || (text[start - 1] === text[start - 1].toLowerCase() && text[start] === text[start].toUpperCase())) &&
								(last + 1 === text.length || text[last + 1] === ' ' || (text[last] === text[last].toLowerCase() && text[last + 1] === text[last + 1].toUpperCase()))
							) {
								text = text.substring(0, start) + newWord + text.substring(last + 1);
							}
						}
						
						// 한국어의 경우 띄어쓰기가 있을 때 좌우에 띄어쓰기가 없으면 무시
						else if (language === 'ko') {
							if (
								spaceCount === 0 || (
									(start === 0 || text[start - 1] === ' ') &&
									(last + 1 === text.length || text[last + 1] === ' ')
								)
							) {
								text = text.substring(0, start) + newWord + text.substring(last + 1);
							}
						}
						
						else {
							text = text.substring(0, start) + newWord + text.substring(last + 1);
						}
					}
				}
			};
			
			db.forEach(function(badWord){
				let newWord = '';
				
				// 영어의 경우 절반 길이로 하트 표시
				/*REPEAT(language === 'en' ? Math.ceil(badWord.length / 2) : badWord.length, () => {
					newWord += '♡';
				});*/
				
				textLength = language === 'en' ? Math.ceil(badWord.length / 2) : badWord.length;
				
				for(var i =0; i<textLength; i++){
					newWord += '♡';
				}
				
				check(badWord, newWord);
				
				// 영어에서는 i를 !로 쓰는 경우가 있음
				if (language === 'en' && badWord.indexOf('i') !== -1) {
					check(badWord.replace(/i/g, '!'), newWord);
				}
			});
			
		}
		
		return text;
});