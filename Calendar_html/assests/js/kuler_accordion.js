;(function($) {

    var Accordion = function(elem, options) {

        var defaults = {
                titleWidth : 48,
                duration : 600,
                easing : 'swing'
            },

            settings = $.extend({}, defaults, options),

            slides = elem.children('.slide'),
            title = slides.children(':first-child'),
            slideCount = slides.length,
            slideWidth = $(elem).width() - slideCount * settings.titleWidth,
            horizontal = true,
            core = {
                activateSlide : function(e) {
                    var $this = $(this);

                    if (!$this.hasClass('selected')) {
                        var tab = {
                            elem : $this,
                            index : title.index($this),
                            next : $this.next(),
                            prev : $this.parent().prev().children('.item-title'),
                            parent : $this.parent()
                        };

                        if (horizontal) {
                            core.animateGroup(tab);
                        } else {
                            core.animate(tab);
                        }
                    }

                    e.preventDefault();
                    return true;
                },

                animate : function(tab) {
                    if (horizontal) {
                        title.removeClass('selected').filter(this.elem).addClass('selected');

                        if (!!tab.index) {
                            tab.elem
                                .add(tab.next)
                                .stop(true)
                                .animate({
                                    left : tab.pos + tab.index * settings.titleWidth
                                },
                                settings.duration,
                                settings.easing);

                            title.removeClass('selected').filter(tab.prev).addClass('selected');
                        }
                    } else {
                        var $elem = $(elem);

                        $elem
                            .find('.item-title.selected')
                                .removeClass('selected')
                                .next()
                                    .slideUp();

                        tab.elem.addClass('selected');
                        tab.next.slideDown();
                    }
                },

                animateGroup : function(triggerTab) {
                    var group = ['left', 'right'];

                    $.each(group, function(index, side) {
                        var filterExpr, left;

                        if (side === 'left')  {
                            filterExpr = ':lt(' + (triggerTab.index + 1) + ')';
                            left = 0;
                        } else {
                            filterExpr = ':gt(' + triggerTab.index + ')';
                            left = slideWidth;
                        }

                        slides
                            .filter(filterExpr)
                            .children('.item-title')
                            .each(function() {
                                var $this = $(this),
                                    tab = {
                                        elem : $this,
                                        index : title.index($this),
                                        next : $this.next(),
                                        prev : $this.parent().prev().children('.item-title'),
                                        pos : left
                                    };

                                core.animate(tab);
                            });
                    });

                    title.removeClass('selected').filter(triggerTab.elem).addClass('selected');
                },

                setClassIE : function(version) {
                    if (version >= 10) return;
                    if (version === 7 || version === 8) {
                        slides.each(function(index) {
                            $(this).addClass('slide-' + index);
                        });
                    }

                    elem.addClass('ie ie' + version);
                },
                makeResponsive: function () {
                    horizontal = $(window).width() > 768 ? true : false;

                    if (horizontal) {
                        slideWidth = $(elem).width() - slideCount * settings.titleWidth

                        elem
                            .removeClass('vertical')
                            .addClass('horizontal');

                        title.next().width(slideWidth - 1);

                        // Styles
                        var maxSize = 0;
                        $('.item-title span, .item-content .product-list', elem).each(function () {
                            var $this = $(this),
                                size = $this.parents('.item-title').length ? $this.outerWidth(true) : $this.outerHeight(true);

                            if (size > maxSize) {
                                maxSize = size;
                            }
                        });

                        elem.height(maxSize);

                        // Position
                        var selected = title.filter('.selected');

                        title.each(function(index) {
                            var $this = $(this),
                                left = index * settings.titleWidth;;

                            if (selected.length) {
                                if (index > title.index(selected)) left += slideWidth;
                            } else {
                                if (index >= 1) left += slideWidth;
                            }

                            $this
                                .css('left', left)
                                .width(maxSize)
                                .next()
                                    .height(maxSize)
                                    .css({
                                        display: 'block',
                                        left : left,
                                        paddingLeft : settings.titleWidth
                                    });
                        });
                    } else {
                        elem
                            .removeClass('horizontal')
                            .addClass('vertical')
                            .css({
                                height: '',
                                width: ''
                            });

                        title.css({
                            width: '100%',
                            height: '',
                            left: ''
                        });

                        slides.find('.item-content')
                            .css({
                                width: '',
                                left: '',
                                height: '',
                                paddingLeft: ''
                            })
                            .each(function () {
                                var $contentItem = $(this);

                                if ($contentItem.prev().hasClass('selected')) {
                                    $contentItem.slideDown();
                                } else {
                                    $contentItem.slideUp();
                                }
                            });
                    }
                },
                init : function() {
                    var ua = navigator.userAgent,
                        index = ua.indexOf('MSIE');

                    // test for ie
                    if (index !== -1) {
                        ua = ua.slice(index + 5, index + 7);
                        core.setClassIE(+ua);
                    }

                    elem.addClass('kulerAccordion');
                    title.eq(0).addClass('selected');

                    slides
                        .children(':first-child')
                        .height(settings.titleWidth);

                    $(window).on('resize', function () {
                        core.makeResponsive();
                    });

                    $(window).trigger('resize');

                    // Bind events
                    title.on('click.kulerAccordion', core.activateSlide);
                }
            };

        core.init();

        this.core = core;
    };

    $.fn.kulerAccordion = function(method) {
        var elem = this,
            instance = elem.data('kulerAccordion');

        if (typeof method === 'object' || !method) {
            return elem.each(function() {
                var kulerAccordion;

                if (instance) return;

                kulerAccordion = new Accordion(elem, method);
                elem.data('kulerAccordion', kulerAccordion);
            });
        }
    };

})(jQuery);